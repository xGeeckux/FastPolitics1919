using FastPolitics1919.Common;
using FastPolitics1919.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FastPolitics1919.Events
{
    public abstract class PlayerEvent : Event
    {
        public Player Player { get; set; }
        
        public virtual void Init(Player player)
        {
            Player = player;
        }

        public override void Fire()
        {
            Player.Events.Add(this);
            Engine.OpenEvent(this);
        }

        public override void OnSelection()
        {
            Player.Events.Remove(this);
            base.OnSelection();
        }
    }
}
