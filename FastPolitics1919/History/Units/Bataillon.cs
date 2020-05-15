using FastPolitics1919.Common;
using FastPolitics1919.History.Units.Attachments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPolitics1919.History.Units
{
    public class Bataillon : Unit
    {
        //- Type
        public override UnitTypes UnitType => UnitTypes.Bataillon;
                
        //- Companies
        public List<Company> Companies
        {
            get
            {
                List<Company> companies = new List<Company>();
                foreach (Unit unit in SubUnits)
                    if (unit is Company company)
                        companies.Add(company);
                return companies;
            }
        }
        
        //- Constructor
        public Bataillon(int location)
        {
            LocationID = location;
            Name = (ID + 1) + ". Bataillon";
        }
        public Bataillon(int location, Person commander)
        {
            LocationID = location;
            Commander = commander;
            Name = (ID + 1) + ". Bataillon '" + commander.Name + "'";
        }
        public Bataillon(int location, string name, Person commander)
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

                    RegularCavalry cav = new RegularCavalry(this);
                    cav.Name = "Kavallerie Zug";
                    cav.SetUnitType(UnitTypes.Platoon);
                    LocalUnitList.Add(cav);

                    RegularInfantry inf = new RegularInfantry(this);
                    inf.Name = "Infanterie Sicherungs-Gruppe";
                    inf.SetUnitType(UnitTypes.Group);
                    LocalUnitList.Add(inf);
                }
                return LocalUnitList;
            }
        }

        //- Create
        public static Bataillon CreateDefault(int location)
        {
            Bataillon bat = new Bataillon(location);
            Engine.Game.Units.Add(bat, bat.ID);

            Company c1 = new Company(location);
            bat.AddSubUnit(c1);
            c1.Parent = bat;
            bat.LinkUp(c1);
            Engine.Game.Units.Add(c1, c1.ID);

            Company c2 = new Company(location);
            bat.AddSubUnit(c2);
            c2.Parent = bat;
            bat.LinkUp(c2);
            Engine.Game.Units.Add(c2, c2.ID);

            Company c3 = new Company(location);
            bat.AddSubUnit(c3);
            c3.Parent = bat;
            bat.LinkUp(c3);
            Engine.Game.Units.Add(c3, c3.ID);

            return bat;
        }
    }
}
