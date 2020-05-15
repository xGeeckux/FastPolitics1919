using FastPolitics1919.Data.Common;
using FastPolitics1919.History.Professions;
using FastPolitics1919.History.Ranks;
using FastPolitics1919.History.Titles;
using LibFastPolitics1919.AVL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPolitics1919.Common
{
    public class Army : GameObject
    {
        public static int Index = 0;
        public GameObject Owner { get; set; }

        public Person Leader { get; set; }

        private SortList<MilitaryRank> MilitaryRanks = new SortList<MilitaryRank>();

        public Army(string name)
        {
            Index++;
            ID = Index;
            Name = name;
            Engine.Game.Armys.Add(this, ID);
            LoadRanks();
        }

        public SortList<Person> Generale
        {
            get
            {
                SortList<Person> persons = new SortList<Person>();
                foreach (Person person in MilitaryPersons)
                    if (person.ArmyRank.Type == RankTypes.General)
                        persons.Add(person, person.ArmyRank.ID);
                return persons;
            }
        }
        public SortList<Person> Stabsoffiziere
        {
            get
            {
                SortList<Person> persons = new SortList<Person>();
                foreach (Person person in MilitaryPersons)
                    if (person.ArmyRank.Type == RankTypes.Stabsoffizier)
                        persons.Add(person, person.ArmyRank.ID);
                return persons;
            }
        }
        public SortList<Person> Offiziere
        {
            get
            {
                SortList<Person> persons = new SortList<Person>();
                foreach (Person person in MilitaryPersons)
                    if (person.ArmyRank.Type == RankTypes.Offizier)
                        persons.Add(person, person.ArmyRank.ID);
                return persons;
            }
        }
        public SortList<Person> Unteroffiziere
        {
            get
            {
                SortList<Person> persons = new SortList<Person>();
                foreach (Person person in MilitaryPersons)
                    if (person.ArmyRank.Type == RankTypes.Unteroffizier)
                        persons.Add(person, person.ArmyRank.ID);
                return persons;
            }
        }
        public SortList<Person> Mannschaftler
        {
            get
            {
                SortList<Person> persons = new SortList<Person>();
                foreach (Person person in MilitaryPersons)
                    if (person.ArmyRank.Type == RankTypes.Mannschaftler)
                        persons.Add(person, person.ArmyRank.ID);
                return persons;
            }
        }
        public List<Person> MilitaryPersons = new List<Person>();

        //- Join/Leave
        public void JoinArmy(Person person)
        {
            if (MilitaryPersons.Contains(person))
                return;
            MilitaryPersons.Add(person);
            person.Army = this;
            PromotePerson(person, GetRanks().Length - 1);
            person.AddProfession(new Soldier(person));
        }
        public void LeaveArmy(Person person)
        {
            if (!MilitaryPersons.Contains(person))
                return;
            if (Leader == person)
                DefineNewLeader();
            MilitaryPersons.Remove(person);
            KickPerson(person);
        }

        //- Promote
        public void PromotePerson(Person person, int id)
        {
            MilitaryRank rank = GetRank(id);
            if (rank != null)
            {
                person.ArmyRank = rank;
                person.RemoveTitle(typeof(MilitaryRankTitle));
                person.AddTitle(typeof(MilitaryRankTitle), new object[] { person.ArmyRank });
            }
        }
        public void DegradePerson(Person person)
        {
            MilitaryRank rank = GetRank(person.ArmyRank.ID + 1);
            if (rank != null)
                PromotePerson(person, rank.ID);
            else
                KickPerson(person);
        }
        public void KickPerson(Person person)
        {
            person.Army = null;
            person.ArmyRank = null;
            person.RemoveTitle(typeof(MilitaryRankTitle));
            person.RemoveProfession(typeof(Commander));
            person.RemoveProfession(typeof(Soldier));
        }

        //- New Leader
        public void DefineNewLeader()
        {
            if (Leader != null)
                if (Leader.HasTitle(typeof(ArmyLeader)))
                    Leader.RemoveTitle(typeof(ArmyLeader));
            if (Generale.Count != 0)
                Leader = Generale.Get()[0];
            else if (Stabsoffiziere.Count != 0)
                Leader = Stabsoffiziere.Get()[0];
            else if (Offiziere.Count != 0)
                Leader = Offiziere.Get()[0];
            else if (Unteroffiziere.Count != 0)
                Leader = Unteroffiziere.Get()[0];
            else if (Mannschaftler.Count != 0)
                Leader = Mannschaftler.Get()[0];
            else
                Leader = null;

            if (Leader != null)
                Leader.AddTitle(typeof(ArmyLeader), new object[] { Leader });
        }
        public void DefineNewLeader(Person person)
        {
            if (Leader != null)
                if (Leader.HasTitle(typeof(ArmyLeader)))
                    Leader.RemoveTitle(typeof(ArmyLeader));
            Leader = person;
            if (Leader != null)
                Leader.AddTitle(typeof(ArmyLeader), new object[] { Leader });
        }

        public MilitaryRank GetRank(int num)
        {
            MilitaryRank rank = MilitaryRanks.Find(num);
            if (rank == null)
                return null;
            MilitaryRank new_rank = new MilitaryRank(rank.ID, rank.Name, rank.Type);
            Engine.CopyProperties(rank, new_rank);
            return new_rank;
        }
        public MilitaryRank[] GetRanks() => MilitaryRanks.Get();

        private void LoadRanks()
        {
            int i = 0;
            //- Generals
            AddRank(new MilitaryRank(i++, "Generalfeldmarschall", RankTypes.General));
            AddRank(new MilitaryRank(i++, "Generaloberst", RankTypes.General));
            AddRank(new MilitaryRank(i++, "General", RankTypes.General));
            AddRank(new MilitaryRank(i++, "Generalleutnant", RankTypes.General));
            AddRank(new MilitaryRank(i++, "Generalmajor", RankTypes.General));
            //- Stab-Officers
            AddRank(new MilitaryRank(i++, "Oberst", RankTypes.Stabsoffizier));
            AddRank(new MilitaryRank(i++, "Oberstleutnant", RankTypes.Stabsoffizier));
            AddRank(new MilitaryRank(i++, "Major", RankTypes.Stabsoffizier));
            //- Officiers
            AddRank(new MilitaryRank(i++, "Hauptmann", RankTypes.Offizier));
            AddRank(new MilitaryRank(i++, "Oberleutnant", RankTypes.Offizier));
            AddRank(new MilitaryRank(i++, "Leutnant", RankTypes.Offizier));
            //- Unteroffizier (m.Pp)
            AddRank(new MilitaryRank(i++, "Stabsfeldwebel", RankTypes.Unteroffizier));
            AddRank(new MilitaryRank(i++, "Hauptfeldwebel", RankTypes.Unteroffizier));
            AddRank(new MilitaryRank(i++, "Oberfeldwebel", RankTypes.Unteroffizier));
            AddRank(new MilitaryRank(i++, "Feldwebel", RankTypes.Unteroffizier));
            //- Unteroffizier (o.Pp)
            AddRank(new MilitaryRank(i++, "Unterfeldwebel", RankTypes.Unteroffizier));
            AddRank(new MilitaryRank(i++, "Unteroffizier", RankTypes.Unteroffizier));
            //- Mannschaft
            AddRank(new MilitaryRank(i++, "Stabsgefreiter", RankTypes.Mannschaftler));
            AddRank(new MilitaryRank(i++, "Obergefreiter", RankTypes.Mannschaftler));
            AddRank(new MilitaryRank(i++, "Gefreiter", RankTypes.Mannschaftler));
            AddRank(new MilitaryRank(i++, "Oberschütze", RankTypes.Mannschaftler));
            AddRank(new MilitaryRank(i++, "Schütze", RankTypes.Mannschaftler));
        }
        private void AddRank(MilitaryRank rank)
        {
            if (rank != null)
                MilitaryRanks.Add(rank, rank.ID);
        }
    }
}
