using FastPolitics1919.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPolitics1919.History.Actions
{
    public class RemoveRelation : FPAction
    {
        //- Name
        public override string Name => "Beziehung verschlechtern";

        //- Constructor
        public RemoveRelation(Person first, Person target) : base(first, target) { }

        //- Prequisition
        public override void Prequisition()
        {
            IndexPosition = 6;
        }
        //- Condition to met
        public override bool Condition()
        {
            return base.Condition() && First.LocationID == Secound.LocationID;
        }
        //- Effect if Pushed
        public override void Effect()
        {
            First.AddRelation(Secound, -25);
            Secound.AddRelation(First, -25);
        }
    }
}
