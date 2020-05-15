using FastPolitics1919.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPolitics1919.History.Governments
{
    [Serializable]
    public class CountryGovernment : Government
    {
        //- Target Country
        public Country Country { get; set; }

        //- Constructor
        public CountryGovernment(Country country)
        {
            Country = country;
        }
    }
}
