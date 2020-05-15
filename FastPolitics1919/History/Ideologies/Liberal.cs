using FastPolitics1919.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPolitics1919.History.Ideologies
{
    public class Liberal : Ideology
    {
        public override string Name => "Liberal";
        public override string Color => "255-255-000";

        public Liberal()
        {
            ID = 5;
        }
    }
}
