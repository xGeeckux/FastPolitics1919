using FastPolitics1919.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPolitics1919.Common
{
    [Serializable]
    public class Citizen : ModifiableObject
    {
        public static int Index;
        public static int AmountPerCitizen = 200;
        public static int MaxCitizens = 4000;

        #region Location
        public Tile Origin => Engine.Game.FindTile(OriginID);
        public Tile Location => Engine.Game.FindTile(LocationID);
        public int OriginID { get; set; }
        public int LocationID { get; set; }
        #endregion

        #region Culture
        public Culture Culture { get; set; }
        #endregion

        #region Politics
        public Ideology Ideology { get; set; }
        private static Random Random = new Random();
        public Party GetVote()
        {
            List<Party> possible_parties = new List<Party>();
            if (Location.CountryOwner != null && Location.CountryOwner.Government != null)
                foreach (Party party in Location.CountryOwner.Government.RegisteredParties)
                    if (party.Ideology == Ideology)
                        possible_parties.Add(party);
            if (Location.Owner != null && Location.Owner.Government != null)
                foreach (Party party in Location.Owner.Government.RegisteredParties)
                    if (party.Ideology == Ideology && !possible_parties.Contains(party))
                        possible_parties.Add(party);
            if (Location is City current)
                foreach (Party party in current.Government.RegisteredParties)
                    if (party.Ideology == Ideology && !possible_parties.Contains(party))
                        possible_parties.Add(party);

            //- Randomness
            if (possible_parties.Count == 0)
                return null;
            if (possible_parties.Count == 1)
                return possible_parties[0];

            int n = Random.Next(0, possible_parties.Count);
            return possible_parties[n];
        }
        #endregion
    }
}
