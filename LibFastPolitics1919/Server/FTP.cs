using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibFastPolitics1919.Server
{
    public class FTP
    {
        public class Data
        {
            public string UriCode { get; set; }
            public string FullPath { get; set; }
            public string Name { get; set; }
        }
        public class Directory : Data
        {
            public List<Data> Children { get; set; }
        }
        public class File : Data
        {
            public string Extension { get; set; }
            public byte[] Content { get; set; }
        }
    }
}
