using FastPolitics1919.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPolitics1919.History.Ideologies
{
    public class Fascist : Ideology
    {
        public override string Name => "Faschist";
        public override string Color => "130-100-000";

        public Fascist()
        {
            ID = 4;
        }
    }
}
