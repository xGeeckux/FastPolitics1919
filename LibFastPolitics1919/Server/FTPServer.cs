using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LibFastPolitics1919.Server
{
    public class FTPServer : TransferProtocolServer
    {
        public override string Sign => "FTPServer";
        //- Server variables
        public ServerInfo ServerInfo { get; set; }
        private string ServerIP { get { return ServerInfo.Adress; } }
        private string Username { get { return ServerInfo.Username; } }
        private string Password { get { return ServerInfo.Password; } }

        //- Constructor (File-Transfer-Protocol Server)
        public FTPServer(ServerInfo info)
        {
            ServerInfo = info;
        }
        
        //- Debug List
        private void ListEverything()
        {
            List<FTP.Data> listing = GetData(ServerIP);
            Write(ServerIP + ", " + listing.Count + " (???) ");
            foreach (FTP.Data dir in listing)
            {
                Write(dir);
            }
        }
        private void Write(FTP.Data data_set)
        {
            Write(data_set.FullPath + "," + " (" + data_set.UriCode + ") ");
            if (data_set is FTP.Directory)
            {
                foreach (FTP.Data children in ((FTP.Directory)data_set).Children)
                {
                    Write(children);
                }
            }

        }

        //- Test Connection
        public bool TestConnection(string addition_path)
        {
            try
            {
                byte[] test = Download(ServerIP + addition_path);
            }
            catch (Exception e)
            {
                Write(e.Message);
                return false;
            }
            return true;
        }

        //- Downloads data information from server
        private List<FTP.Data> GetData(string full_path)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(full_path);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            request.Credentials = new NetworkCredential(Username, Password);

            string[] files_or_folders = null;
            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    files_or_folders = reader.ReadToEnd().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                }
            }

            List<FTP.Data> DataList = new List<FTP.Data>();

            foreach (string txt in files_or_folders)
            {
                string original = txt;
                string data_name = txt.Substring(49, txt.Length - 49);

                string file_check = original.Substring(0, 1);
                if (file_check == "d")
                {
                    //- Directory
                    FTP.Directory dir = new FTP.Directory();
                    dir.UriCode = original;
                    dir.Name = data_name;
                    dir.FullPath = full_path + "/" + dir.Name;
                    dir.Children = new List<FTP.Data>();
                    DataList.Add(dir);
                    foreach (FTP.Data directory in GetData(dir.FullPath))
                    {
                        dir.Children.Add(directory);
                    }
                }
                else
                {
                    //- File
                    FTP.File file = new FTP.File();
                    file.UriCode = original;
                    file.Name = data_name;
                    file.FullPath = full_path + "/" + file.Name;
                    string[] splitted = data_name.Split('.');
                    file.Extension = "";
                    file.Content = Download(file.FullPath);
                    if (splitted.Length == 2)
                        file.Extension = splitted[1];
                    DataList.Add(file);
                }
            }

            return DataList;
        }

        //- Download full file
        public byte[] Download(string full_path)
        {
            using (WebClient request = new WebClient())
            {
                request.Credentials = new NetworkCredential(Username, Password);
                Write($"Downloading.. " + full_path);
                byte[] fileData = request.DownloadData(full_path);
                Write($"Download complete!");
                return fileData;
            }
        }
        public void Upload(string full_path, byte[] bytes)
        {
            using (WebClient request = new WebClient())
            {
                request.Credentials = new NetworkCredential(Username, Password);
                Write($"Uploading.. " + full_path);
                request.UploadData(full_path, bytes);
                Write($"Upload complete!");
            }
        }

        //- Download Files and Folders to Client
        public void DownloadToClient(string client_directory)
        {
            List<FTP.Data> every_data = GetData(ServerIP);

            List<FTP.Data> real = new List<FTP.Data>();
            SetRealList(real, every_data);

            foreach (FTP.Data data in real)
            {
                if (data is FTP.Directory)
                {
                    FTP.Directory local = (FTP.Directory)data;
                    string full_real_path = local.FullPath;
                    full_real_path = full_real_path.Substring(ServerIP.Length, full_real_path.Length - ServerIP.Length);
                    if (!Directory.Exists(client_directory + @"\" + full_real_path))
                    {
                        Directory.CreateDirectory(client_directory + @"\" + full_real_path);
                    }
                }
                if (data is FTP.File)
                {
                    FTP.File local = (FTP.File)data;
                    string full_real_path = local.FullPath;
                    full_real_path = full_real_path.Substring(ServerIP.Length, full_real_path.Length - ServerIP.Length);
                    if (!File.Exists(client_directory + @"\" + full_real_path))
                    {
                        CreateClientFile(client_directory + @"\" + full_real_path, local.Content);
                    }
                }
            }
        }
        private void SetRealList(List<FTP.Data> list, List<FTP.Data> children)
        {
            foreach (FTP.Data data in children)
            {
                list.Add(data);
                if (data is FTP.Directory)
                {
                    SetRealList(list, ((FTP.Directory)data).Children);
                }
            }
        }
        private void CreateClientFile(string path, byte[] content)
        {
            using (FileStream file = File.Create(path))
            {
                file.Write(content, 0, content.Length);
                file.Close();
            }
        }
    }
}
