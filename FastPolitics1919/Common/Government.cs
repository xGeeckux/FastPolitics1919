using FastPolitics1919.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPolitics1919.Common
{
    [Serializable]
    public abstract class Government : GameObject
    {
        #region Representants
        public virtual string TitleCancelor => "Kanzler";
        public Person Cancelor { get; set; }
        public List<Party> RulingParties { get; set; }
        #endregion

        #region Parlament
        public List<ParlamentSeat> Seats = new List<ParlamentSeat>();
        public List<PartyAmount> PartyAmounts = new List<PartyAmount>();
        public int MaxSeats { get; set; }
        #endregion

        #region Parties
        public List<Party> RegisteredParties = new List<Party>();
        #endregion

        //- Constructor
        public Government()
        {
            RegisteredParties = new List<Party>();
        }
        
        public class PartyAmount
        {
            public Party Party { get; set; }
            public int Seats { get; set; }

            public PartyAmount(Party party, int seats)
            {
                Party = party;
                Seats = seats;
            }
        }
    }
}
