using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace FastPolitics1919.Common
{
    public static class Tooltips
    {
        private static StackPanel GetPanel()
        {
            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;
            return stackPanel;
        }
        private static Label GetBasic(string family)
        {
            Label lbl = new Label();
            lbl.Padding = new System.Windows.Thickness(0);
            lbl.FontFamily = new FontFamily(family);
            lbl.Foreground = Brushes.White;
            lbl.FontSize = 14;
            lbl.Margin = new System.Windows.Thickness(0, 0, 5, 0);
            return lbl;
        }

        //- Person
        //-     Money
        public static StackPanel PersonChangeMoney(Person person, double money)
        {
            StackPanel stackPanel = GetPanel();
            
            Label lbl_value = GetBasic("Bodoni MT");
            lbl_value.Content = money;
            if (money >= 0)
                lbl_value.Foreground = Brushes.Green;
            else
                lbl_value.Foreground = Brushes.Red;
            stackPanel.Children.Add(lbl_value);

            return stackPanel;
        }
    }
}
