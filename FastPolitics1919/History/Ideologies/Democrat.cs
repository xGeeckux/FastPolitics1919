using FastPolitics1919.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPolitics1919.History.Ideologies
{
    public class Democrat : Ideology
    {
        public override string Name => "Demokrat";
        public override string Color => "110-160-235";

        public Democrat()
        {
            ID = 6;
        }
    }
}
