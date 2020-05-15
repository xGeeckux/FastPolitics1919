using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibFastPolitics1919.Server
{
    public class TransferProtocolServer
    {
        public virtual string Sign => "TransferProtocolServer";

        public void Write(object text)
        {
            Console.WriteLine(text.ToString());
        }
    }
}
