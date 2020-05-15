using FastPolitics1919.Common;
using FastPolitics1919.History.Ranks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FastPolitics1919.History.Titles
{
    public class ArmyLeader : Title
    {
        public Person Person { get; set; }
        public override BitmapImage Image => Person.ArmyRank.Icon;

        public ArmyLeader(Person person)
        {
            Person = person;
            Name = "Oberbefehlshaber";
            TitleValue = Person.ArmyRank.ID + 200;
        }
    }
}
