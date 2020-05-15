using FastPolitics1919.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPolitics1919.History.Actions
{
    public class Follower : FPAction
    {
        //- Name
        public override string Name
        {
            get
            {
                if (First.Follows == Secound)
                    return "Kein Anhänger mehr von " + Secound.Name + " sein";
                else
                    return "Anhänger von " + Secound.Name + " werden";
            }
        }

        //- Constructor
        public Follower(Person first, Person target) : base(first, target) { }

        //- Prequisition
        public override void Prequisition()
        {
            IndexPosition = 2;
        }
        //- Condition to met
        public override bool Condition()
        {
            return base.Condition() && First.LocationID == Secound.LocationID;
        }
        //- Effect if Pushed
        public override void Effect()
        {
            if (First.Follows == Secound)
                First.ChangeFollower(null);
            else
                First.ChangeFollower(Secound);
        }
    }
}
