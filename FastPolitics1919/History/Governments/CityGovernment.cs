using FastPolitics1919.Common;
using FastPolitics1919.History.Professions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPolitics1919.History.Governments
{
    [Serializable]
    public class CityGovernment : Government
    {
        //- Title
        public override string TitleCancelor => "Bürgermeister";

        //- Target Country
        public City City { get; set; }

        #region ElectionInfos
        public Round LastElection { get; set; }
        public Round NextElection { get; set; }
        public int GovernmentPeriode { get; set; } = 4 * 12 * 4;
        #endregion

        //- Constructor
        public CityGovernment(City city)
        {
            City = city;
            MaxSeats = 12;
        }

        //- Election
        public void Election()
        {
            AbendParlament();

            #region Update Seats
            List<Party> parties = new List<Party>();
            List<int> partie_values = new List<int>();

            foreach (Citizen citizen in City.Citizens)
            {
                Party local = citizen.GetVote();
                if (local == null)
                    continue;
                if (parties.Contains(local))
                    partie_values[parties.FindIndex(x => x.ID == local.ID)] += 1;
                else
                {
                    parties.Add(local);
                    partie_values.Add(1);
                }
            }
            
            int[] seat_nums = new int[partie_values.Count];
            int sum_votes = 0;
            for (int i = 0; i < partie_values.Count; i++)
                sum_votes += partie_values[i];
            for (int i = 0; i < parties.Count; i++)
            {
                double relation = (double)partie_values[i] / sum_votes;
                int num_of_legal_seats = (int)(MaxSeats * relation);
                for (int j = 0; j < num_of_legal_seats; j++)
                {
                    ParlamentSeat seat = new ParlamentSeat();
                    seat.Person = GetPerson(parties[i]);
                    seat_nums[i]++;
                    Seats.Add(seat);
                }
            }

            //- Regardles of the %-num of any party, one seat is guaranted
            for (int i = 0; i < parties.Count; i++)
            {
                if(seat_nums[i] == 0)
                {
                    ParlamentSeat seat = new ParlamentSeat();
                    seat.Person = GetPerson(parties[i]);
                    seat_nums[i]++;
                    MaxSeats++;
                    Seats.Add(seat);
                }
            }

            for (int i = 0; i < parties.Count; i++)
                PartyAmounts.Add(new PartyAmount(parties[i], seat_nums[i]));
            #endregion

            RulingParties = GetMajority();
            Cancelor = GetPerson(RulingParties[0]);

            //- New Round-Dates
            LastElection = Engine.Game.Current;
            NextElection = new Round(LastElection.Number + GovernmentPeriode);
            Log.Write("Election over:\n" + "Winner: '" + Cancelor.Party.Name + "' with new '" + TitleCancelor + "' " + Cancelor.Name);
        }
        protected List<Party> GetMajority()
        {
            int[] nums = new int[this.PartyAmounts.Count];
            for (int i = 0; i < nums.Length; i++)
                nums[i] = PartyAmounts[i].Seats;
            int index = Engine.GetBiggestIndex(nums);
            return new List<Party>() { PartyAmounts[index].Party };
        }
        protected Person GetPerson(Party party)
        {
            Person[] possible = party.GetPossiblePersons();
            for (int i = 0; i < possible.Length; i++)
            {
                if (!possible[i].HasProfession(typeof(Politician)))
                {
                    possible[i].AddProfession(new Politician(party));
                    return possible[i];
                }
            }
            foreach (Person person in City.Persons)
            {
                if (person.Party == party && !person.HasProfession(typeof(Politician)))
                {
                    person.AddProfession(new Politician(party));
                    return person;
                }
            }

            Person p1 = new Person
            {
                Name = Engine.Game.GetRandomName(),
                Party = party,
                Ideology = party.Ideology,
                Culture = Engine.Game.FindCulture(Cultures.Cultures.Deutsch),
                OriginID = City.ID,
                LocationID = City.ID
            };
            p1.JoinParty(party);
            Engine.Game.Citizens.Add(p1, ID);

            return p1;
        }

        //- Abendon Parlament
        public void AbendParlament()
        {
            ClearSeats();
            Cancelor = null;
        }
        protected void ClearSeats()
        {
            if (Seats == null)
                return;

            foreach (ParlamentSeat seat in Seats)
                seat.Person.RemoveProfession(typeof(Politician));
            Seats.Clear();
            PartyAmounts.Clear();
        }
    }
}
