using FastPolitics1919.Data.Common;
using FastPolitics1919.Interface.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace FastPolitics1919.Common
{
    public abstract class Event : GameObject
    {
        private static int Index = 0;

        public EventSubWindow Window { get; set; }

        //- Constructor
        public Event()
        {
            ID = Index++;
            Effects = new List<EffectButton>();
        }

        //- Effects
        public List<EffectButton> Effects { get; set; }

        //- Condition
        public bool Condition() => IsTriggeredOnly ? IsTriggeredOnly : EventCondition();
        protected virtual bool EventCondition() => false;
        public virtual bool IsTriggeredOnly => false;

        public abstract void Fire();

        public void Open()
        {
            Window = new EventSubWindow(this);
        }
        public void Close()
        {
            Window.Close();
        }

        public virtual void OnSelection()
        {
            Close();
        }
    }
    public class EffectButton : Button
    {
        public delegate void EffectMethode();

        public bool IsHistorical { get; set; }

        private EffectMethode Methode { get; set; }
        private Event Event { get; set; }
        public EffectButton(string name, EffectMethode effect, Event @event)
        {
            Load(name, effect, @event);
        }
        public EffectButton(string name, object tooltip, EffectMethode effect, Event @event)
        {
            ToolTip = tooltip;
            Load(name, effect, @event);
        }
        private void Load(string name, EffectMethode effect, Event @event)
        {
            Event = @event;
            Methode = effect;
            Click += LocalClick;

            Background = Brushes.LightCoral;
            Padding = new System.Windows.Thickness(0);
            Content = name;
        }

        private void LocalClick(object sender, System.Windows.RoutedEventArgs e)
        {
            Methode();
            Event.OnSelection();
        }
    }
}
