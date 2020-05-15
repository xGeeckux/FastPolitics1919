using FastPolitics1919.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPolitics1919.History.Ideologies
{
    public class Communist : Ideology
    {
        public override string Name => "Kommunist";
        public override string Color => "200-000-000";

        public Communist()
        {
            ID = 11;
        }
    }
}
