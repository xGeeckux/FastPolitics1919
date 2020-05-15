using FastPolitics1919.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPolitics1919.History.Units
{
    public class Regiment : Unit
    {
        //- Type
        public override UnitTypes UnitType => UnitTypes.Regiment;

        //- Companies
        public List<Bataillon> Regiments
        {
            get
            {
                List<Bataillon> bataillons = new List<Bataillon>();
                foreach (Unit unit in SubUnits)
                    if (unit is Bataillon bat)
                        bataillons.Add(bat);
                return bataillons;
            }
        }

        //- Constructor
        public Regiment(int location)
        {
            LocationID = location;
            Name = (ID + 1) + ". Regiment";
        }
        public Regiment(int location, Person commander)
        {
            LocationID = location;
            Commander = commander;
            Name = (ID + 1) + ". Regiment '" + commander.Name + "'";
        }
        public Regiment(int location, string name, Person commander)
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

        //- Generate
        public static Regiment CreateDefault(int location)
        {
            Regiment reg = new Regiment(location);
            Engine.Game.Units.Add(reg, reg.ID);

            Bataillon bat1 = Bataillon.CreateDefault(location);
            reg.AddSubUnit(bat1);
            bat1.Parent = reg;
            reg.LinkUp(bat1);

            Bataillon bat2 = Bataillon.CreateDefault(location);
            reg.AddSubUnit(bat2);
            bat2.Parent = reg;
            reg.LinkUp(bat2);

            Bataillon bat3 = Bataillon.CreateDefault(location);
            reg.AddSubUnit(bat3);
            bat3.Parent = reg;
            reg.LinkUp(bat3);

            return reg;
        }
    }
}
