using FastPolitics1919.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FastPolitics1919.History.Actions
{
    public class LeaveParty : FPAction
    {
        //- Name
        public override string Name => "Partei verlassen";
        public override bool ForeignInteraction => false;

        //- Constructor
        public LeaveParty(Person first, Person target) : base(first, target) { }

        //- Prequisition
        public override void Prequisition()
        {
            IndexPosition = 3;
        }
        //- Condition to met
        public override bool Condition()
        {
            return base.Condition() && First.Party != null;
        }
        //- Effect if Pushed
        public override void Effect()
        {
            First.LeaveParty();
        }
    }
}
