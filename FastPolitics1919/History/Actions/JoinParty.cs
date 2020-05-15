using FastPolitics1919.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FastPolitics1919.History.Actions
{
    public class JoinParty : FPAction
    {
        //- Name
        public override string Name => "Anfragen: Partei beitritt";

        //- Constructor
        public JoinParty(Person first, Person target) : base(first, target) { }

        //- Prequisition
        public override void Prequisition()
        {
            IndexPosition = 3;
        }
        //- Condition to met
        public override bool Condition()
        {
            return base.Condition() && First.LocationID == Secound.LocationID 
                && Secound.Knows(First) && First.Party == null
                && First.Party != Secound.Party && Secound.Party != null;
        }
        //- Effect if Pushed
        public override void Effect()
        {
            if (Secound.GetRelationTo(First) >= 25)
            {
                First.AddRelation(Secound, 10);
                Secound.AddRelation(First, 10);
                First.JoinParty(Secound.Party);
            }
            else
            {
                MessageBox.Show("Beziehung zu niedrig.");
            }
        }
    }
}
