using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastPolitics1919.Common;

namespace FastPolitics1919.History.Units.Hqs
{
    public class RedHQ : HQ
    {
        public override string NatoCounter => "hq_rot_1";

        public RedHQ(Unit parent) : base(parent)
        {
            Name = "Kommando Abteilung";
        }
    }
}
