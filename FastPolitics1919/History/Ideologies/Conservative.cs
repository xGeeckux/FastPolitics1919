using FastPolitics1919.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPolitics1919.History.Ideologies
{
    public class Conservative : Ideology
    {
        public override string Name => "Konservativ";
        public override string Color => "000-000-000";

        public Conservative()
        {
            ID = 1;
        }
    }
}
