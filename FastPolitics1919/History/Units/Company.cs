using FastPolitics1919.Common;
using FastPolitics1919.History.Units.Attachments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPolitics1919.History.Units
{
    public class Company : Unit
    {
        //- Type
        public override UnitTypes UnitType => UnitTypes.Kompanie;

        //- Constructor
        public Company(int location)
        {
            LocationID = location;
            Name = (ID + 1) + ". Kompanie";
        }
        public Company(int location, Person commander)
        {
            LocationID = location;
            Commander = commander;
            Name = (ID + 1) + ". Kompanie '" + commander.Name + "'";
        }
        public Company(int location, string name, Person commander)
        {
            LocationID = location;
            Commander = commander;
            Name = name + " '" +  commander.Name + "'";
        }

        //- LocalUnits
        public override List<Unit> LocalUnits
        {
            get
            {
                if (LocalUnitList == null)
                {
                    LocalUnitList = base.LocalUnits;
                    LocalUnitList.Add(new RegularCavalry(this));
                    LocalUnitList.Add(new RegularInfantry(this));
                    LocalUnitList.Add(new RegularInfantry(this));
                    LocalUnitList.Add(new RegularInfantry(this));
                }
                return LocalUnitList;
            }
        }
    }
}
