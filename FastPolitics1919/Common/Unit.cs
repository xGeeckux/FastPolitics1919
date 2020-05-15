using FastPolitics1919.Data.Common;
using FastPolitics1919.Gfx;
using FastPolitics1919.History.Orders;
using FastPolitics1919.History.Professions;
using FastPolitics1919.History.Units;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FastPolitics1919.Common
{
    public abstract class Unit : MapObject
    {
        public static int Index = 0;
        public static List<Unit> HasMoved = new List<Unit>();

        public GameObject Owner { get; set; }
        public Person Commander { get; set; }

        //- Map
        public string Color
        {
            get
            {
                if (Owner != null && Owner is Country country)
                    return country.RGBColor;
                if (IsHostile)
                    return Brushes.DarkRed.Color.R + "-" + Brushes.DarkRed.Color.G + "-" + Brushes.DarkRed.Color.B;
                return null;
            }
        }
        public Border BorderParent { get; set; }
        
        public bool IsHostile { get; set; } = false;
        public bool IsFighting { get; set; }

        //- Battle Stats
        public virtual BattleStats BattleValues
        {
            get
            {
                BattleStats stats = new BattleStats() { MaxOrganisation = 0, MaxStrength = 0 };
                int divisor = 0;
                foreach (Unit unit in SubUnits)
                {
                    if (!unit.IsLinked)
                        continue;
                    stats.SoftAttack += unit.BattleValues.SoftAttack;
                    stats.SoftDefence += unit.BattleValues.SoftDefence;
                    stats.MaxSoftDefence += unit.BattleValues.MaxSoftDefence;
                    stats.SoftAttackDefence += unit.BattleValues.SoftAttackDefence;

                    stats.HardAttack += unit.BattleValues.HardAttack;
                    stats.HardDefence += unit.BattleValues.HardDefence;
                    stats.HardAttackDefence += unit.BattleValues.HardAttackDefence;

                    stats.CurOrganisation += unit.BattleValues.CurOrganisation;
                    stats.MaxOrganisation += unit.BattleValues.MaxOrganisation;

                    stats.CurStrength += unit.BattleValues.CurStrength;
                    stats.MaxStrength += unit.BattleValues.MaxStrength;

                    divisor++;
                }

                foreach (Unit unit in LocalUnits)
                {
                    if (!unit.IsLinked || unit is HQ)
                        continue;
                    stats.CurOrganisation += unit.BattleValues.CurOrganisation;
                    stats.MaxOrganisation += unit.BattleValues.MaxOrganisation;

                    stats.CurStrength += unit.BattleValues.CurStrength;
                    stats.MaxStrength += unit.BattleValues.MaxStrength;

                    divisor++;
                }

                //- Division
                stats.SoftAttack /= divisor;
                stats.SoftDefence /= divisor;
                stats.MaxSoftDefence /= divisor;
                stats.SoftAttackDefence /= divisor;

                stats.HardAttack /= divisor;
                stats.HardDefence /= divisor;
                stats.HardAttackDefence /= divisor;

                stats.CurOrganisation /= divisor;
                stats.MaxOrganisation /= divisor;
                return stats;
            }
        }
        public abstract UnitTypes UnitType { get; }
        public virtual int CurStrength
        {
            get
            {
                int strength = 0;
                strength += BattleValues.CurStrength;
                //foreach (Unit unit in SubUnits)
                //    if (unit.IsLinked)
                //        strength += unit.BattleValues.CurStrength;
                //foreach (Unit unit in LocalUnits)
                //    if (unit.IsLinked)
                //        strength += unit.BattleValues.CurStrength;
                return strength;
            }
        }
        public virtual int MaxStrength
        {
            get
            {
                int strength = 0;
                strength += BattleValues.MaxStrength;
                //foreach (Unit unit in SubUnits)
                //    if (unit.IsLinked)
                //        strength += unit.BattleValues.MaxStrength;
                //foreach (Unit unit in LocalUnits)
                //    if (unit.IsLinked)
                //        strength += unit.BattleValues.MaxStrength;
                return strength;
            }
        }
        public virtual double CurOrganisation
        {
            get
            {
                double organisation = 0;
                organisation += BattleValues.CurOrganisation;
                //foreach (Unit unit in SubUnits)
                //    if (unit.IsLinked)
                //        organisation += unit.BattleValues.CurOrganisation;
                //foreach (Unit unit in LocalUnits)
                //    if (unit.IsLinked && !(unit is HQ))
                //        organisation += unit.BattleValues.CurOrganisation;
                return organisation;
            }
        }
        public virtual double MaxOrganisation
        {
            get
            {
                double organisation = 0;
                organisation += BattleValues.MaxOrganisation;
                //foreach (Unit unit in SubUnits)
                //    if (unit.IsLinked)
                //        organisation += unit.BattleValues.MaxOrganisation;
                //foreach (Unit unit in LocalUnits)
                //    if (unit.IsLinked && !(unit is HQ))
                //        organisation += unit.BattleValues.MaxOrganisation;
                return organisation;
            }
        }
        //- Strength
        public virtual void AddStrength(int num)
        {
            int count = 0;
            foreach (Unit unit in LocalUnits)
                if (unit is Attachment)
                    count++;

            double single = (double)num / count;
            foreach (Unit unit in LocalUnits)
                if (unit is Attachment attach)
                    attach.AddStrength((int)Math.Round(single, 0));
        }
        public virtual void SetStrength(int num)
        {
            int count = 0;
            foreach (Unit unit in LocalUnits)
                if (unit is Attachment)
                    count++;

            double single = (double)num / count;
            foreach (Unit unit in LocalUnits)
                if (unit is Attachment attach)
                    attach.SetStrength((int)Math.Round(single, 0));
        }
        //- Organisation
        public virtual void AddOrganisation(double num)
        {
            int count = 0;
            foreach (Unit unit in LocalUnits)
                if (unit is Attachment)
                    count++;

            double single = (double)num / count;
            foreach (Unit unit in LocalUnits)
                if (unit is Attachment attach)
                    attach.AddOrganisation(single);
        }
        public virtual void SetOrganisation(double num)
        {            
            foreach (Unit unit in LocalUnits)
                if (unit is Attachment attach)
                    attach.SetOrganisation(num);
        }

        //- Order Of Battle
        public Unit Parent { get; set; }
        public bool IsLinked { get; set; }

        //- SubUnits
        public List<Unit> SubUnits = new List<Unit>();
        public void AddSubUnit(Unit unit)
        {
            if (unit.Parent != null)
                unit.Parent.RemoveSubUnit(unit);
            SubUnits.Add(unit);
            unit.Parent = this;
        }
        public void RemoveSubUnit(Unit unit)
        {
            if (!SubUnits.Contains(unit))
                return;
            SubUnits.Remove(unit);
            unit.Parent = null;
        }

        //- LocalUnits
        public HQ HQ { get; set; }
        protected List<Unit> LocalUnitList { get; set; }
        public virtual List<Unit> LocalUnits
        {
            get
            {
                if (LocalUnitList == null)
                {
                    LocalUnitList = new List<Unit>();
                    if (HQ == null)
                        HQ = new HQ(this);
                    LocalUnitList.Add(HQ);
                }
                return LocalUnitList;
            }
        }
        public void AddLocalUnit(Attachment attachment)
        {
            List<Unit> load = LocalUnits;
            if (LocalUnitList != null)
                LocalUnitList.Add(attachment);
        }
        public void ClearLocalUnits()
        {
            List<Unit> load = LocalUnits;
            if (LocalUnitList != null)
            {
                LocalUnitList.Clear();
                LocalUnitList.Add(HQ);
            }
        }

        //- Local Persons
        public List<Person> Persons = new List<Person>();
        public void AddPerson(Person person)
        {
            if (person.ArmyRank != null)
                Persons.Add(person);
        }
        public void RemovePerson(Person person)
        {
            if (Persons.Contains(person))
                Persons.Remove(person);
        }

        //- Symbol
        public virtual BitmapImage Symbol
        {
            get
            {
                switch (UnitType)
                {
                    default: return null;
                    case UnitTypes.Team:
                        return Images.IconUnitSizeTeam;
                    case UnitTypes.Group:
                        return Images.IconUnitSizeGroup;
                    case UnitTypes.Platoon:
                        return Images.IconUnitSizePlatoon;
                    case UnitTypes.Kompanie:
                        return Images.IconUnitSizeCompany;
                    case UnitTypes.Bataillon:
                        return Images.IconUnitSizeBataillony;
                    case UnitTypes.Regiment:
                        return Images.IconUnitSizeRegiment;
                    case UnitTypes.Brigarde:
                        return Images.IconUnitSizeCompany;
                    case UnitTypes.Division:
                        return Images.IconUnitSizeDivision;
                    case UnitTypes.Korps:
                        return Images.IconUnitSizeCorps;
                    case UnitTypes.Armee:
                        return Images.IconUnitSizeArmy;
                    case UnitTypes.Armeegruppe:
                        return Images.IconUnitSizeArmygroup;
                }
            }
        }
        public virtual string NatoCounter => "infantry";
        public BitmapImage IconCounter => Images.FromPath(Images.military_counters + NatoCounter);

        //- Loaction
        public Tile Location => Engine.Game.FindTile(LocationID);
        public int LocationID { get; set; }

        //- Constructor
        public Unit()
        {
            Index++;
            ID = Index;
        }

        //- Kill this Unit
        public void KillEveryone()
        {

        }

        //- Moves Unit
        public void MoveTo(Tile tile)
        {
            //- Debug
            TeleportTo(tile);
            HasMoved.Add(this);
        }
        public void TeleportTo(Tile tile)
        {
            if (Commander != null && Commander.LocationID == LocationID)
                Commander.TeleportTo(tile);
            LocationID = tile.ID;

            //- Persons
            foreach (Person person in Persons)
                if (person.LocationID == LocationID)
                    person.TeleportTo(tile);

            //- SubUnits
            foreach (Unit unit in SubUnits)
                if (unit.IsLinked)
                    unit.TeleportTo(tile);

            //- Check for Controller Change
            if (Owner != null && Owner is Country country)
            {
                if (Location.CountryOwner != null && Location.CountryOwner != country)
                {
                    Location.SetController(country);
                }
            }
        }

        //- Compine and Disorder
        public void LinkUp(Unit unit)
        {
            if (unit.LocationID != LocationID)
                return;
            if (!SubUnits.Contains(unit))
                return;
            unit.IsLinked = true;
            Engine.Map.Update();
        }
        public void LinkOut(Unit unit)
        {
            if (!SubUnits.Contains(unit))
                return;
            unit.IsLinked = false;
            Engine.Map.Update();
        }

        //- SetCommander
        public void DefineNewCommander(Person person)
        {
            if (person.HasProfession(typeof(Commander)))
            {
                person.RemoveProfession(typeof(Commander));
            }
            Commander = person;
            person.AddProfession(new Commander(person, this));
        }
        public void RemoveCommander()
        {
            if (Commander == null)
                return;
            Commander.RemoveProfession(typeof(Commander));
        }

        //- Update
        public void Update()
        {
            if (IsFighting)
                return;
            Regenerate();

            if (!HasMoved.Contains(this))
                UpdateOrders();

            #region Debug
            //- Randomness
            if (Engine.Map.CurrentSelected == this)
                return;
            bool moves = Convert.ToBoolean(Random.Next(0, 1 + 1));
            if (moves && Orders.Count == 0)
            {
                List<Tile> Neighbours = Location.GetNeighbours();
                int i = Random.Next(0, Neighbours.Count);
                Orders.Add(new MoveOrder(this, Neighbours[i].ID));
            }
            #endregion
        }

        //- Orders
        public List<Order> Orders = new List<Order>();
        public void UpdateOrders()
        {
            if (Orders.Count == 0)
                return;
            Order order = Orders[0];
            order.Effect();
            Orders.Remove(order);
        }

        //- Updates
        protected static Random Random = new Random();
        protected virtual int RegenerationRate => 20;
        private void Regenerate()
        {
            //- Organisation
            double _ch = (double)BattleValues.CurStrength / BattleValues.MaxStrength;
            double full = (double)BattleValues.MaxOrganisation / 3;
            BattleValues.CurOrganisation += full * _ch * (Commander == null ? 0.75 : 1) * (Owner != null && Owner is Country ? 0.5 : 1.25);
            if (BattleValues.CurOrganisation > BattleValues.MaxOrganisation)
                BattleValues.CurOrganisation = BattleValues.MaxOrganisation;

            //- Manpower
            if (Owner != null && Owner is Country)
            {
                AddStrength(RegenerationRate);
            }
        }

        public class BattleStats
        {
            #region Battle Numbers
            //- Soft
            public double SoftAttack { get; set; }          //- SoftAttack
            public double SoftDefence { get; set; }         //- Defence
            public double MaxSoftDefence { get; set; }
            public double SoftAttackDefence { get; set; }   //- Breakthrough
            //- Hard
            public double HardAttack { get; set; }          //- HardAttack
            public double HardDefence { get; set; }         //- Armor
            public double HardAttackDefence { get; set; }   //- Piercing
            //- Strenght
            public int CurStrength { get; set; }
            public int MaxStrength { get; set; } = 200;
            //- Organisation
            public double CurOrganisation { get; set; }
            public double MaxOrganisation { get; set; } = 60;
            #endregion
        }
    }
}
