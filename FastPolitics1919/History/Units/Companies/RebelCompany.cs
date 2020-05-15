using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastPolitics1919.Common;
using FastPolitics1919.History.Units.Attachments;
using FastPolitics1919.History.Units.Hqs;

namespace FastPolitics1919.History.Units.Companies
{
    public class RebelCompany : Company
    {
        public RebelCompany(int location) : base(location)
        {
        }
        public RebelCompany(int location, Person commander) : base(location, commander)
        {
        }
        public RebelCompany(int location, string name, Person commander) : base(location, name, commander)
        {
        }

        //- LocalUnits
        public override List<Unit> LocalUnits
        {
            get
            {
                if (LocalUnitList == null)
                {
                    LocalUnitList = new List<Unit>();
                    LocalUnitList.Add(HQ = new RedHQ(this));
                    LocalUnitList.Add(new RegularRebel(this));
                }
                return LocalUnitList;
            }
        }
    }
}
