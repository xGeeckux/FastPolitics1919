using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FastPolitics1919.Data.Common
{
    public class ModifiableObject : GameObject
    {
        #region Lists
        public List<SingleModifierItem> GetItems(Modifiers modifier)
        {
            List<SingleModifierItem> local = new List<SingleModifierItem>();

            foreach (Modifier mod in Modifiers)
                foreach (ModifierItem item in mod.Items)
                    if (item.Is(modifier))
                    {
                        local.Add(new SingleModifierItem(mod, item));
                        break;
                    }

            return local;
        }
        public List<Modifier> GetModifiers(Modifiers modifier)
        {
            List<Modifier> local = new List<Modifier>();

            foreach (Modifier mod in Modifiers)
                foreach (ModifierItem single in mod.Items)
                    if (single.Is(modifier))
                    {
                        local.Add(mod);
                        break;
                    }

            return local;
        }
        public ModifierItem GetModifier(Modifiers modifier)
        {
            ModifierItem item = new ModifierItem(modifier, 0);
            foreach (Modifier mod in Modifiers)
            {
                foreach (ModifierItem single in mod.Items)
                {
                    if (single.Is(modifier))
                        item += single;
                }
            }
            return item;
        }
        public List<Modifier> Modifiers
        {
            get
            {
                List<Modifier> modifiers = new List<Modifier>();
                LoadEveryModifier();
                modifiers.AddRange(OtherModifiers);
                modifiers.AddRange(LocalModifiers);
                return modifiers;
            }
        }

        protected List<Modifier> OtherModifiers = new List<Modifier>();
        private List<Modifier> LocalModifiers = new List<Modifier>();
        #endregion

        #region LocalMethodes
        protected virtual void LoadEveryModifier()
        {
            OtherModifiers.Clear();
        }
        protected void Append(ModifiableObject @object)
        {
            OtherModifiers.AddRange(GetModifiersFrom(@object));
        }
        #endregion

        #region Graphics
        //- Gets every ModifierItem
        public StackPanel GetModifierPanel(Modifier modifier)
        {
            StackPanel panel = new StackPanel();
            panel.Margin = new Thickness(-4);

            Label lbl = GetBasic("Bell MT");
            lbl.FontWeight = FontWeights.Bold;
            lbl.Content = modifier.Name + " gibt folgende Effekte:";
            panel.Children.Add(lbl);

            foreach (ModifierItem item in modifier.Items)
                panel.Children.Add(GetHorPanel(item.Name, item));
            return panel;
        }
        //- Gets every Modifier for Property
        public StackPanel GetPropertyPanel(Modifiers modifier)
        {
            StackPanel panel = new StackPanel();
            panel.Margin = new Thickness(-6,-4,-6,-4);
            

            Label lbl = GetBasic("Bell MT");
            lbl.Margin = new Thickness(5);
            lbl.FontStyle = FontStyles.Italic;
            lbl.Content = "Wird modifiziert durch:";
            panel.Children.Add(lbl);

            foreach (SingleModifierItem item in GetItems(modifier))
            {
                StackPanel local = GetHorPropertyPanel(item.Name, item.Item);
                local.Margin = new Thickness(40, 0, 0, 0);
                panel.Children.Add(local);
            }
            return panel;
        }
        //- Get Hor-Panel
        private StackPanel GetHorPanel(string name, ModifierItem value)
        {
            StackPanel panel = new StackPanel();
            panel.Orientation = Orientation.Horizontal;

            Label lbl_name = GetBasic("Bell MT");
            lbl_name.FontWeight = FontWeights.Bold;
            lbl_name.Content = name;
            panel.Children.Add(lbl_name);

            Label lbl_value = GetBasic("Bodoni MT");
            lbl_value.Content = value.GetValue();
            switch (value.RelativeValue)
            {
                case RelativeValue.Positiv:
                    if (value.Value >= 0)
                        lbl_value.Foreground = Brushes.Green;
                    else
                        lbl_value.Foreground = Brushes.Red;
                    break;
                case RelativeValue.Negativ:
                    if (value.Value > 0)
                        lbl_value.Foreground = Brushes.Red;
                    else
                        lbl_value.Foreground = Brushes.Green;
                    break;
            }
            panel.Children.Add(lbl_value);

            return panel;
        }
        private StackPanel GetHorPropertyPanel(string name, ModifierItem value)
        {
            StackPanel panel = new StackPanel();
            panel.Orientation = Orientation.Horizontal;

            Label lbl_name = GetBasic("Bell MT");
            lbl_name.FontWeight = FontWeights.Bold;
            lbl_name.Content = name;
            lbl_name.Foreground = Brushes.Orange;
            panel.Children.Add(lbl_name);

            Label lbl_mit = GetBasic("Bell MT");
            lbl_mit.Content = "mit";
            panel.Children.Add(lbl_mit);

            Label lbl_value = GetBasic("Bodoni MT");
            lbl_value.Content = value.GetValue();
            switch (value.RelativeValue)
            {
                case RelativeValue.Positiv:
                    if (value.Value >= 0)
                        lbl_value.Foreground = Brushes.Green;
                    else
                        lbl_value.Foreground = Brushes.Red;
                    break;
                case RelativeValue.Negativ:
                    if (value.Value > 0)
                        lbl_value.Foreground = Brushes.Red;
                    else
                        lbl_value.Foreground = Brushes.Green;
                    break;
            }
            panel.Children.Add(lbl_value);

            return panel;
        }
        private Label GetBasic(string family)
        {
            Label lbl = new Label();
            lbl.Padding = new System.Windows.Thickness(0);
            lbl.FontFamily = new FontFamily(family);
            lbl.Foreground = Brushes.White;
            lbl.FontSize = 14;
            lbl.Margin = new System.Windows.Thickness(0, 0, 5, 0);
            return lbl;
        }
        
        //- Tooltips
        public Border GetTooltip(Modifiers modifier, double value, double mod_value)
        {
            Border border = new Border();
            border.Background = Brushes.Black;
            border.BorderBrush = Brushes.Gold;
            border.BorderThickness = new Thickness(2);
            border.CornerRadius = new CornerRadius(5);

            StackPanel panel = new StackPanel();
            panel.Margin = new Thickness(5);
            border.Child = panel;

            //- Name
            Label lbl_modifier_name = GetBasic("Bell MT");
            lbl_modifier_name.FontWeight = FontWeights.Bold;
            lbl_modifier_name.Foreground = Brushes.Yellow;
            lbl_modifier_name.Content = Modifier.List[(int)modifier].Name;
            panel.Children.Add(lbl_modifier_name);

            StackPanel hor_panel_1 = new StackPanel();
            hor_panel_1.Orientation = Orientation.Horizontal;
            hor_panel_1.Margin = new Thickness(10, 0, 0, 0);
            panel.Children.Add(hor_panel_1);

            Label lbl1 = GetBasic("Bell MT");
            lbl1.Content = "Basis mit";
            hor_panel_1.Children.Add(lbl1);

            Label lbl2 = GetBasic("Bell MT");
            lbl2.Content = Math.Round(value, 2);
            lbl2.FontWeight = FontWeights.Bold;
            lbl2.Foreground = Brushes.Yellow;
            hor_panel_1.Children.Add(lbl2);

            StackPanel hor_panel_2 = new StackPanel();
            hor_panel_2.Orientation = Orientation.Horizontal;
            hor_panel_2.Margin = new Thickness(10, 0, 0, 0);
            panel.Children.Add(hor_panel_2);

            Label lbl3 = GetBasic("Bell MT");
            lbl3.Content = "Aktuell bei";
            hor_panel_2.Children.Add(lbl3);

            Label lbl4 = GetBasic("Bell MT");
            lbl4.Content = "(mit Modifiern)";
            lbl4.FontStyle = FontStyles.Italic;
            lbl4.Foreground = Brushes.LightGray;
            hor_panel_2.Children.Add(lbl4);

            Label lbl5 = GetBasic("Bell MT");
            lbl5.Content = Math.Round(mod_value, 2);
            lbl5.FontWeight = FontWeights.Bold;
            lbl5.Foreground = Brushes.Yellow;
            hor_panel_2.Children.Add(lbl5);

            Label lbl_ = GetBasic("Bell MT");
            lbl_.Content = "-----------------";
            panel.Children.Add(lbl_);
            
            Label lbl_modifier_text = GetBasic("Bell MT");
            lbl_modifier_text.Content = "Wird aktuell modifiziert durch:";
            lbl_modifier_text.FontStyle = FontStyles.Italic;
            lbl_modifier_text.Foreground = Brushes.YellowGreen;
            panel.Children.Add(lbl_modifier_text);

            ModifierItem sum = new ModifierItem(modifier, 0);
            foreach (SingleModifierItem item in GetItems(modifier))
            {
                StackPanel local = GetHorPropertyPanel(item.Name, item.Item);
                sum += item.Item;
                local.Margin = new Thickness(40, 0, 0, 0);
                panel.Children.Add(local);
            }

            StackPanel hor_panel_3 = new StackPanel();
            hor_panel_3.Orientation = Orientation.Horizontal;
            panel.Children.Add(hor_panel_3);

            Label lbl6 = GetBasic("Bell MT");
            lbl6.Content = "Vollständig mit";
            hor_panel_3.Children.Add(lbl6);

            Label lbl7 = GetBasic("Bell MT");
            lbl7.Content = "(";
            lbl7.Margin = new Thickness(0);
            hor_panel_3.Children.Add(lbl7);

            Label lbl_value = GetBasic("Bodoni MT");
            lbl_value.Content = sum.GetValue();
            lbl_value.Margin = new Thickness(0);
            switch (sum.RelativeValue)
            {
                case RelativeValue.Positiv:
                    if (sum.Value >= 0)
                        lbl_value.Foreground = Brushes.Green;
                    else
                        lbl_value.Foreground = Brushes.Red;
                    break;
                case RelativeValue.Negativ:
                    if (sum.Value > 0)
                        lbl_value.Foreground = Brushes.Red;
                    else
                        lbl_value.Foreground = Brushes.Green;
                    break;
            }
            hor_panel_3.Children.Add(lbl_value);

            Label lbl8 = GetBasic("Bell MT");
            lbl8.Content = ")";
            lbl8.Margin = new Thickness(0);
            hor_panel_3.Children.Add(lbl8);

            return border;
        }
        #endregion

        #region Modifier Add/Remove
        public void AddModifier(Modifier modifier)
        {
            LocalModifiers.Add(modifier);
        }
        public void RemoveModifier(Modifier modifier)
        {
            LocalModifiers.Remove(modifier);
        }
        private List<Modifier> GetModifiersFrom(ModifiableObject @object)
        {
            return @object.LocalModifiers;
        }
        #endregion
    }
}
