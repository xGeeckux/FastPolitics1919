using FastPolitics1919.Data.Common;
using FastPolitics1919.Gfx;
using FastPolitics1919.History.Actions;
using FastPolitics1919.History.Professions;
using FastPolitics1919.History.Ranks;
using FastPolitics1919.History.Titles;
using FastPolitics1919.Interface.Game;
using LibFastPolitics1919.AVL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace FastPolitics1919.Common
{
    [Serializable]
    public class Person : Citizen
    {
        //- Picture
        public string ImageName { get; set; }
        public BitmapImage Image
        {
            get
            {
                if (ImageName == "" || ImageName == null)
                    return Images.IconPerson;
                BitmapImage img;
                return (img = Images.FromPath(Images.person_potrai + ImageName)) == null ? Images.IconPerson : img;
            }
        }

        #region Economics
        public double Money { get; set; }
        public List<BankAccount> BankAccounts = new List<BankAccount>();
        public BankAccount DefaultBankAccount { get; set; }
        public void AddMoney(double money)
        {
            Money += money;
            if (Engine.CurrentPerson == this)
                Engine.GameInterface.Update();
        }
        #endregion

        #region Relations
        public List<Relation> Relations = new List<Relation>();
        public List<Person> Followers = new List<Person>();
        public Person Follows { get; set; }
        public void ChangeFollower(Person person)
        {
            if (Follows != null)
            {
                Follows.Followers.Remove(this);
                AddRelation(Follows, -40);
            }
            Follows = person;
            if (person != null)
            {
                person.Followers.Add(this);
                AddRelation(Follows, 40);
            }
        }
        #endregion

        #region Honor
        public List<Title> Titles = new List<Title>();
        public Title GetHighestTitle()
        {
            if (Titles.Count == 0)
                return null;
            Title highest = Titles[0];
            for (int i = 1; i < Titles.Count; i++)
            {
                if (Titles[i].TitleValue > highest.TitleValue)
                {
                    highest = Titles[i];
                }
            }
            return highest;
        }
        public void AddTitle(Type type, object[] args)
        {
            Title title = Engine.CreateClass<Title>(type, args);
            Titles.Add(title);
        }
        public bool HasTitle(Type type)
        {
            foreach (Title title in Titles)
                if (title.GetType() == type)
                    return true;
            return false;
        }
        private int GetTitleIndex(Type type)
        {
            for (int i = 0; i < Titles.Count; i++)
                if (Titles[i].GetType() == type)
                    return i;
            return -1;
        }
        public void RemoveTitle(Type type)
        {
            if (!HasTitle(type))
                return;
            Titles.RemoveAt(GetTitleIndex(type));
        }
        #endregion

        #region Army
        public Army Army { get; set; }
        public MilitaryRank ArmyRank { get; set; }
        public void JoinArmy(Army army)
        {
            army.JoinArmy(this);
        }
        public void LeaveArmy()
        {
            if (Army != null)
                Army.LeaveArmy(this);
        }
        #endregion

        #region Unit

        #endregion

        #region Profession
        public SortList<Profession> Professions = new SortList<Profession>();
        public Profession MainProfession { get; set; }
        public void AddProfession(Profession profession)
        {
            //- If already contained
            if (Professions.Find(profession.Importance) != null)
                return;

            Professions.Add(profession, profession.Importance);
            if (MainProfession == null || MainProfession.Importance < profession.Importance)
                MainProfession = profession;
        }
        public void RemoveProfession(Type type)
        {
            Profession pro = null;
            foreach (Profession profession in Professions.Get())
                if (profession.GetType() == type)
                    pro = profession;

            if (pro == null)
                return;

            pro.RemoveEffect();
            Professions.Remove(pro.Importance);
            if (MainProfession == pro)
            {
                if (Professions.Length == 0)
                    MainProfession = null;
                Profession[] un = Professions.Get();
                List<Profession> list = un.ToList();
                list.Reverse();
                MainProfession = list[0];
            }
        }
        public bool HasProfession()
        {
            return MainProfession != null;
        }
        public bool HasProfession(Type type)
        {
            foreach (Profession profession in Professions.Get())
                if (profession.GetType() == type)
                    return true;
            return false;
        }
        #endregion

        #region Events
        public List<Event> Events = new List<Event>();
        #endregion

        //- Constructor
        public Person()
        {
            Index++;
            ID = Index;
        }

        //- Actions
        #region Actions
        //-     -Other Persons
        public void TalkTo(Person person)
        {
            if (!Knows(person))
            {
                AddRelation(person, -10);   //- Stranger
                person.AddRelation(this, -10);
            }
        }
        public void AddRelation(Person person, double value)
        {
            Relation relation = RelationTo(person);
            if (relation == null)
                Relations.Add(new Relation() { Target = person, Value = value });
            else
                relation.Value += value;
        }
        public bool Knows(Person person)
        {
            if (person == this)
                return true;
            foreach (Relation relation in Relations)
                if (relation.Target == person)
                    return true;
            return false;
        }
        private Relation RelationTo(Person person)
        {
            if (person == this)
                return new Relation() { Target = this, Value = 200 };
            foreach (Relation relation in Relations)
                if (relation.Target == person)
                    return relation;
            return null;
        }
        public double GetRelationTo(Person person)
        {
            if (person == this)
                return 200;
            foreach (Relation relation in Relations)
                if (relation.Target == person)
                    return relation.Value;
            return 0;
        }
        //-     -Party
        public Party Party { get; set; }
        public void JoinParty(Party party)
        {
            if (party == null)
                return;
            Party = party;
            AddProfession(new Politician(party));
            if (party.Founder == this || party.Leader == this)
            {
                party.AddPerson(this);
                AddTitle(typeof(PartyFounder), new object[] { Party });
            }
        }
        public void FoundParty(string name, string short_name, Ideology ideology)
        {
            Party party = new Party(name, short_name, this, ideology);
            JoinParty(party);
            if (Location is City city)
                party.RegisterParty(city);
        }
        public void LeaveParty()
        {
            if (Party == null)
                return;
            if (Party.Leader == this)
                Party.DefineNewLeader();
            if (Party.IsPossiblePerson(this))
                Party.RemovePerson(this);
            Party = null;
        }
        //-      -Movement
        public void TeleportTo(Tile tile)
        {
            LocationID = tile.ID;
        }
        public void MoveTo(Tile tile)
        {
            TeleportTo(tile);
        }
        #endregion

        //- PersonWindow
        private void Open()
        {
            Engine.Open(new PersonSubWindow(this));
        }
        public void PersonClick(object sender, RoutedEventArgs e)
        {
            Open();
        }
    }
}
