using FastPolitics1919.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FastPolitics1919.History.Professions
{
    public class Soldier : Profession
    {
        public override double Salary => 4;
        public override int Importance => 30;

        public Person Person { get; set; }

        public Soldier(Person person)
        {
            Person = person;
            Name = "Soldat";
        }

        public override StackPanel GetPersonPanel()
        {
            StackPanel panel = base.GetPersonPanel();

            Label rang = this.HighliteText();
            rang.Content = Person.ArmyRank.Type;
            panel.Children.Add(rang);

            Label txt = this.NormalText();
            txt.Content = "in der Armee von";
            panel.Children.Add(txt);

            Label army = this.WhiteText();
            army.Content = Person.Army.Owner.Name;
            panel.Children.Add(army);

            return panel;
        }
    }
}
