using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibFastPolitics1919.Server
{
    public class ServerInfo
    {
        //- Default variables
        public string IPAdress { get; set; }
        public string Adress { get { return IPAdress + Addition; } }
        public string Username { get; set; }
        public string Password { get; set; }

        public string Addition { get; set; } = "";

        //- Main constructor
        public ServerInfo(string adress, string username, string password)
        {
            IPAdress = adress;
            Username = username;
            Password = password;
        }

        //- Test
        public bool TestConnection()
        {
            return true;
        }
    }
}
