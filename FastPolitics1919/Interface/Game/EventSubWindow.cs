using FastPolitics1919.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPolitics1919.Interface.Game
{
    public class EventSubWindow : SubWindow
    {
        //- Variable
        public Event Event { get; set; }
        public EventWindow Window { get; set; }

        //- Constructor
        public EventSubWindow(Event @event)
        {
            Event = @event;
            Constructor(Window = new EventWindow(@event));
        }

        //- Load Event specific content
        protected override void InternLoad()
        {

        }

        public override void Update() => Window.Update();

        //- City Window Exit
        protected override void ExitWindow()
        {

        }
    }
}
