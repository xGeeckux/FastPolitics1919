using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPolitics1919.Common
{
    public class Round
    {
        public int Number { get; set; }

        public Round(int round)
        {
            Number = round;
        }

        public override string ToString()
        {
            return Number + ". Runde";
        }
    }
}
