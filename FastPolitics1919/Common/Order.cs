using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPolitics1919.Common
{
    public abstract class Order
    {
        public string Name { get; set; }
        public abstract string OrderText { get; }

        public abstract void Effect();
    }
}
