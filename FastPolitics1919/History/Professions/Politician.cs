using FastPolitics1919.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FastPolitics1919.History.Professions
{
    public class Politician : Profession
    {
        public override double Salary => 4;
        public override int Importance => 25;

        public Party Party { get; set; }
        public Politician(Party party)
        {
            Party = party;
            Name = "Politiker";
        }

        public override StackPanel GetPersonPanel()
        {
            StackPanel panel = base.GetPersonPanel();

            Label txt1 = this.NormalText();
            txt1.Content = "Politiker in der";
            panel.Children.Add(txt1);

            Label rang = this.HighliteText();
            rang.Content = Party.Name;
            panel.Children.Add(rang);

            Label txt2 = this.NormalText();
            txt2.Content = "-Partei";
            panel.Children.Add(txt2);

            return panel;
        }
    }
}
