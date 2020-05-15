using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPolitics1919.Common
{
    public class Relation
    {
        public Person Target { get; set; }

        public double Value { get; set; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
