using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPolitics1919.Data
{
    public abstract class LogWriter
    {
        //- Default Sign
        public abstract string Sign { get; }

        //- Write-Methode
        public void Write(object text)
        {
            Log.Write(this, text);
        }
    }
}
