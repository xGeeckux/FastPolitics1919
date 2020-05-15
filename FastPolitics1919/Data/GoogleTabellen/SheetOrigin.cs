using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPolitics1919.Data.GoogleTabellen
{
    public class SheetOrigin
    {
        //- Url from Table
        public string SheetUrl { get; set; }
        //- Tab from Table
        public string SheetTab { get; set; }

        //- Datetimes
        public DateTime BaseRequest { get; set; }
        public DateTime LastRequested { get; set; }
    }
}
