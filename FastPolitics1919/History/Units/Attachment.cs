using FastPolitics1919.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FastPolitics1919.History.Units
{
    public class Attachment : Unit
    {
        public override BattleStats BattleValues => Stats;
        protected BattleStats Stats { get; set; }

        public override UnitTypes UnitType => LocalType;
        private UnitTypes LocalType { get; set; }

        public Attachment(Unit parent)
        {
            Parent = parent;
            LocalType = UnitTypes.Platoon;
            Name = "Infanterie-Abteilung";
            Stats = new BattleStats();
            Stats.CurOrganisation = 60;
            Stats.MaxOrganisation = 60;
            Stats.CurStrength = (int)LocalType;
            Stats.MaxStrength = (int)LocalType;
            IsLinked = true;
        }

        public override void AddStrength(int num)
        {
            Stats.CurStrength += num;
            if (Stats.CurStrength > Stats.MaxStrength)
                Stats.CurStrength = Stats.MaxStrength;
            if (Stats.CurStrength < 0)
                Stats.CurStrength = 0;
        }
        public override void SetStrength(int num)
        {
            Stats.CurStrength = num;
            if (Stats.CurStrength > Stats.MaxStrength)
                Stats.CurStrength = Stats.MaxStrength;
            if (Stats.CurStrength < 0)
                Stats.CurStrength = 0;
        }
        public override void AddOrganisation(double num)
        {
            Stats.CurOrganisation += num;
            if (Stats.CurOrganisation > Stats.MaxOrganisation)
                Stats.CurOrganisation = Stats.MaxOrganisation;
            if (Stats.CurOrganisation < 0)
                Stats.CurOrganisation = 0;
        }
        public override void SetOrganisation(double num)
        {
            Stats.CurOrganisation = num;
            if (Stats.CurOrganisation > Stats.MaxOrganisation)
                Stats.CurOrganisation = Stats.MaxOrganisation;
            if (Stats.CurOrganisation < 0)
                Stats.CurOrganisation = 0;
        }

        public void SetUnitType(UnitTypes type)
        {
            LocalType = type;
            Stats.MaxStrength = (int)type;
            SetStrength(Stats.CurStrength);
        }
    }
}
