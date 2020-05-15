using FastPolitics1919.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPolitics1919.History.Units.Attachments
{
    public class RegularRebel : Attachment
    {
        public RegularRebel(Unit parent) : base(parent)
        {
            Name = "Rebellen Gruppierung";
            LoadRebelInfantry();
        }
        public void LoadRebelInfantry()
        {
            Stats.SoftAttack = 5;
            Stats.SoftDefence = 20;
            Stats.MaxSoftDefence = Stats.SoftDefence;
            Stats.SoftAttackDefence = 3;

            Stats.HardAttack = 0;
            Stats.HardDefence = 0;
            Stats.HardAttackDefence = 0;
        }
    }
}
