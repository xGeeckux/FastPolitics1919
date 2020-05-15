using FastPolitics1919.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FastPolitics1919.History.Professions
{
    public class Commander : Profession
    {
        public override double Salary => 5;
        public override int Importance => 100;

        public Person Person { get; set; }
        public Unit Unit { get; set; }

        public Commander(Person person, Unit unit)
        {
            Unit = unit;
            Person = person;
            Name = "Befehlshaber";
        }

        public override void RemoveEffect()
        {
            Unit.RemoveCommander();
        }

        public override StackPanel GetPersonPanel()
        {
            StackPanel panel = base.GetPersonPanel();

            Label rang = this.HighliteText();
            rang.Content = Name;
            panel.Children.Add(rang);

            Label txt = this.NormalText();
            txt.Content = "der";
            panel.Children.Add(txt);

            Label army = this.WhiteText();
            army.Content = Unit.Name;
            panel.Children.Add(army);

            return panel;
        }
    }
}
