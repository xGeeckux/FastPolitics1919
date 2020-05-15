using FastPolitics1919.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FastPolitics1919.History.Actions
{
    public class TalkTo : FPAction
    {
        //- Name
        public override string Name => "Rede mit " + Secound.Name;

        //- Constructor
        public TalkTo(Person first, Person target) : base(first, target) { }

        //- Prequisition
        public override void Prequisition()
        {
            IndexPosition = 1;
        }
        //- Condition to met
        public override bool Condition()
        {
            return base.Condition() && !First.Knows(Secound) && First.LocationID == Secound.LocationID;
        }
        //- Effect if Pushed
        public override void Effect()
        {
            First.TalkTo(Secound);
        }
    }
}
