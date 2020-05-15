using FastPolitics1919.Data.Common;
using LibFastPolitics1919.AVL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPolitics1919.Common
{
    [Serializable]
    public class Party : GameObject
    {
        //- Count Index
        public static int Index = 0;

        #region BaseInfo
        public string ShortName { get; set; }
        public Ideology Ideology { get; set; }
        public string Color { get; set; }
        #endregion

        #region Leaders
        public Person Founder { get; set; }
        public Person Leader { get; set; }
        private List<int> Position = new List<int>();
        private List<Person> PossiblePersons = new List<Person>();
        public void AddPerson(Person person)
        {
            AddPerson(person, Position.Count);
        }
        public void AddPerson(Person person, int position)
        {
            if (PossiblePersons.Contains(person))
                return;
            if (position > PossiblePersons.Count)
                position = PossiblePersons.Count;

            for (int i = 0; i < PossiblePersons.Count; i++)
            {
                if (Position[i] >= position)
                {
                    Position[i]++;
                }
            }
            PossiblePersons.Add(person);
            Position.Add(position);
        }
        public void ChangePersonPosition(Person person, int position)
        {
            if (!PossiblePersons.Contains(person))
                throw new Exception("person ist nicht in der Liste enthalten");
            RemovePerson(person);
            AddPerson(person, position);
        }
        public void RemovePerson(Person person)
        {
            if (!PossiblePersons.Contains(person))
                return;
            int index = 0;
            for (int i = 0; i < PossiblePersons.Count; i++)
            {
                if (PossiblePersons[i] == person)
                    index = i;
            }
            int pos_value = Position[index];
            PossiblePersons.RemoveAt(index);
            Position.RemoveAt(index);
            for (int i = 0; i < PossiblePersons.Count; i++)
            {
                if (Position[i] > pos_value)
                {
                    Position[i]--;
                }
            }
        }
        public bool IsPossiblePerson(Person person)
        {
            return PossiblePersons.Contains(person);
        }
        public int GetCurrentPosition(Person person)
        {
            if (PossiblePersons.Contains(person))
                for (int i = 0; i < PossiblePersons.Count; i++)
                    if (PossiblePersons[i] == person)
                        return Position[i];
            return -1;
        }
        public Person[] GetPossiblePersons()
        {
            SortList<Person> person = new SortList<Person>();
            for (int i = 0; i < PossiblePersons.Count; i++)
                person.Add(PossiblePersons[i], Position[i]);
            return person.Get();
        }
        public void DefineNewLeader()
        {
            if (Leader != null)
            {
                if (IsPossiblePerson(Leader))
                {
                    RemovePerson(Leader);
                }
            }
            Person[] persons;
            if ((persons = GetPossiblePersons()).Length > 0)
            {
                Leader = persons[0];
                AddPerson(Leader, 0);
                Leader.JoinParty(this);
                return;
            }
            Leader = null;
        }
        public void DefineNewLeader(Person person)
        {
            Leader = person;
            Leader.JoinParty(this);
            if (!IsPossiblePerson(person))
                AddPerson(person, 0);
            else
                ChangePersonPosition(person, 0);
        }
        #endregion

        #region Members
        public List<Person> Members
        {
            get
            {
                List<Person> persons = new List<Person>();
                foreach (Citizen citizen in Engine.Game.Citizens.Get())
                    if (citizen is Person person && person.Party != null && person.Party == this)
                        persons.Add(person);
                return persons;
            }
        }
        public int MemberCount => Members.Count;
        #endregion

        #region Organisation
        public void RegisterParty(City city)
        {
            if (city.Government != null)
                city.Government.RegisteredParties.Add(this);
        }
        public void RegisterParty(Province province)
        {
            if (province.Government != null)
                province.Government.RegisteredParties.Add(this);
        }
        public void RegisterParty(Country country)
        {
            if (country.Government != null)
                country.Government.RegisteredParties.Add(this);
        }
        #endregion

        //- Constructor
        public Party(string name, string short_name, Person founder, Ideology ideology)
        {
            ID = Index;
            Index++;

            Name = name;
            ShortName = short_name;
            if (founder != null)
            {
                Founder = founder;
                Leader = Founder;
                AddPerson(Leader, 0);
            }
            Ideology = ideology;
            Color = Ideology.Color;
            Engine.Game.Parties.Add(this, ID);
        }
        public Party(int id, string name, string short_name, Person founder, Ideology ideology)
        {
            ID = id;
            Name = name;
            ShortName = short_name;
            if (founder != null)
            {
                Founder = founder;
                Leader = Founder;
                AddPerson(Leader, 0);
            }
            Ideology = ideology;
            Color = Ideology.Color;
            Engine.Game.Parties.Add(this, ID);
        }
    }
}
