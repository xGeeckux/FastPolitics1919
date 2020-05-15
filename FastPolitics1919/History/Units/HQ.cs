using FastPolitics1919.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FastPolitics1919.History.Units
{
    public class HQ : Unit
    {
        public override BattleStats BattleValues => Stats;
        public BattleStats Stats { get; set; }

        public override UnitTypes UnitType => UnitTypes.HQ;

        public override BitmapImage Symbol => Parent.Symbol;
        public override string NatoCounter => "hq_1";

        public HQ(Unit parent)
        {
            Parent = parent;
            Name = "HQ-Abteilung";
            Stats = new BattleStats();
            Stats.CurOrganisation = 25;
            Stats.MaxOrganisation = 25;
            Stats.CurStrength = (int)UnitTypes.HQ;
            Stats.MaxStrength = (int)UnitTypes.HQ;
            IsLinked = true;
        }
    }
}
