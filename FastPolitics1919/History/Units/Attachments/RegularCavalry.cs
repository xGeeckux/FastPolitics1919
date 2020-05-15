using FastPolitics1919.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPolitics1919.History.Units.Attachments
{
    public class RegularCavalry : Attachment
    {
        public override string NatoCounter => "cavalry";
        public RegularCavalry(Unit parent) : base(parent)
        {
            Name = "Kavallerie-Abteilung";
            LoadDefaultCavalry();
        }
        public void LoadDefaultCavalry()
        {
            Stats.SoftAttack = 33;
            Stats.SoftDefence = 50;
            Stats.MaxSoftDefence = Stats.SoftDefence;
            Stats.SoftAttackDefence = 25;

            Stats.HardAttack = 0;
            Stats.HardDefence = 0;
            Stats.HardAttackDefence = 0;
        }
    }
}
