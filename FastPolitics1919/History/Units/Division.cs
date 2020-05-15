using FastPolitics1919.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPolitics1919.History.Units
{
    public class Division : Unit
    {
        //- Type
        public override UnitTypes UnitType => UnitTypes.Division;

        //- Companies
        public List<Regiment> Regiments
        {
            get
            {
                List<Regiment> regiments = new List<Regiment>();
                foreach (Unit unit in SubUnits)
                    if (unit is Regiment regiment)
                        regiments.Add(regiment);
                return regiments;
            }
        }

        //- Constructor
        public Division(int location)
        {
            LocationID = location;
            Name = (ID + 1) + ". Division";
        }
        public Division(int location, Person commander)
        {
            LocationID = location;
            Commander = commander;
            Name = (ID + 1) + ". Division '" + commander.Name + "'";
        }
        public Division(int location, string name, Person commander)
        {
            LocationID = location;
            Commander = commander;
            Name = name + " '" + commander.Name + "'";
        }

        //- LocalUnits
        public override List<Unit> LocalUnits
        {
            get
            {
                if (LocalUnitList == null)
                {
                    LocalUnitList = base.LocalUnits;
                    LocalUnitList.Add(new Attachment(this));
                }
                return LocalUnitList;
            }
        }
    }
}
