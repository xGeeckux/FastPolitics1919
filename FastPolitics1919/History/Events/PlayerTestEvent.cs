using FastPolitics1919.Common;
using FastPolitics1919.Data.Common;
using FastPolitics1919.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FastPolitics1919.History.Events
{
    public class PlayerTestEvent : PlayerEvent
    {
        public PlayerTestEvent()
        {
            Effects.Add(new EffectButton(
                "Das ist wohl die einzige Wahl"
                ,Option, this));
        }

        protected override bool EventCondition()
        {
            return true;
        }
        public override bool IsTriggeredOnly => false;

        public void Option()
        {
            Player.AddMoney(10);
            MessageBox.Show("Geld + 10");
        }
    }
}
