using FastPolitics1919.Data.Common;
using FastPolitics1919.Data.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FastPolitics1919.Common
{
    public abstract class Profession : GameObject
    {
        //- Economy
        public abstract double Salary { get; }

        //- Importance
        public abstract int Importance { get; }

        //- GUI
        public virtual StackPanel GetPersonPanel()
        {
            StackPanel panel = new StackPanel();
            panel.Orientation = Orientation.Horizontal;
            return panel;
        }

        //- Triggerd on Remove
        public virtual void RemoveEffect()
        {

        }

        protected Label HighliteText()
        {
            Label lbl = new Label();
            //<Label Content="Rank-Name" Padding="0" Foreground="#E5C5B091" 
            //FontFamily ="Bell MT" Margin="0,4,0,0" FontWeight="Bold" 
            //FontStyle ="Italic" FontSize="16" />
            lbl.Padding = new System.Windows.Thickness(0);
            lbl.Foreground = ColorHandler.ColorFromHex("E5C5B091");
            lbl.FontFamily = new System.Windows.Media.FontFamily("Bell MT");
            lbl.Margin = new System.Windows.Thickness(0, 4, 0, 0);
            lbl.FontWeight = FontWeights.Bold;
            lbl.FontStyle = FontStyles.Italic;
            lbl.FontSize = 16;
            return lbl;
        }
        protected Label NormalText()
        {
            Label lbl = new Label();
            //<Label Padding="0" Content="in der Armee von" Foreground="#E5F1B660" 
            //FontFamily ="Bell MT" Margin="5,4,5,0" FontSize="16" />
            lbl.Padding = new System.Windows.Thickness(0);
            lbl.Foreground = ColorHandler.ColorFromHex("E5F1B660");
            lbl.FontFamily = new System.Windows.Media.FontFamily("Bell MT");
            lbl.Margin = new System.Windows.Thickness(5, 4, 5, 0);
            lbl.FontSize = 16;
            return lbl;
        }
        protected Label WhiteText()
        {
            Label lbl = new Label();
            // <Label Content="Country-Army-Name" Padding="0" Foreground="#E5FFFFFF" 
            //FontFamily ="Bodoni MT" Margin="2,4,0,0" FontWeight="Bold" FontSize="16" />
            lbl.Padding = new System.Windows.Thickness(0);
            lbl.Foreground = ColorHandler.ColorFromHex("E5FFFFFF");
            lbl.FontFamily = new System.Windows.Media.FontFamily("Bodoni MT");
            lbl.Margin = new System.Windows.Thickness(2, 4, 0, 0);
            lbl.FontWeight = FontWeights.Bold;
            lbl.FontSize = 16;
            return lbl;
        }
    }
}
