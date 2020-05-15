using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPolitics1919.Common
{
    [Serializable]
    public class ParlamentSeat
    {
        public Person Person { get; set; }
        public Party Party => Person.Party;
        public Ideology Ideology => Person.Ideology;
    }
}
