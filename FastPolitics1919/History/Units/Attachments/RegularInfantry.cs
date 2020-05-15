using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastPolitics1919.Common;

namespace FastPolitics1919.History.Units.Attachments
{
    public class RegularInfantry : Attachment
    {
        public RegularInfantry(Unit parent) : base(parent)
        {
            LoadDefaultInfantry();
        }
        public void LoadDefaultInfantry()
        {
            Stats.SoftAttack = 25;
            Stats.SoftDefence = 75;
            Stats.MaxSoftDefence = Stats.SoftDefence;
            Stats.SoftAttackDefence = 15;

            Stats.HardAttack = 0;
            Stats.HardDefence = 3;
            Stats.HardAttackDefence = 0;
        }
    }
}
