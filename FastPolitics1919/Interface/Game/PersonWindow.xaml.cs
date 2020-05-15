using FastPolitics1919.Common;
using FastPolitics1919.Data.Handlers;
using FastPolitics1919.Gfx;
using FastPolitics1919.History.Actions;
using FastPolitics1919.History.Ranks;
using FastPolitics1919.History.Titles;
using LibFastPolitics1919.AVL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FastPolitics1919.Interface.Game
{
    /// <summary>
    /// Interaktionslogik für PersonWindow.xaml
    /// </summary>
    public partial class PersonWindow : Window
    {
        //- Variables
        public Person Person { get; set; }

        //- Constructor
        public PersonWindow(Person person)
        {
            Person = person;
            Icon = Images.IconQuestionmark;
            InitializeComponent();
            Load();
        }

        //- Loading
        private void Load()
        {
            Update();
            LoadTabActions();
        }

        //- Actions
        private void LoadTabActions()
        {
            img_action_header.Source = Images.IconQuestionmark;

            tree_actions.Items.Clear();

            SortList<FPAction> legit = new SortList<FPAction>();
            foreach (Type action_type in Engine.Game.EveryAction)
            {
                object[] args = new object[] { Engine.CurrentPerson, Person };
                FPAction fp_action = Engine.CreateClass<FPAction>(action_type, args);
                if (fp_action.Condition())
                    legit.Add(fp_action, fp_action.IndexPosition);
            }
            foreach (FPAction action in legit.Get())
                AddAction(action);
        }
        private void AddAction(FPAction action)
        {
            TreeViewItem item = new TreeViewItem();
            item.Margin = new Thickness(0, 5, 0, 0);
            item.Header = GetActionButton(action.Name, action.HexColor, action.Icon, action.Click);
            tree_actions.Items.Add(item);

            //TalkTo person = new TalkTo(new Person() { Name = "a" }, new Person() { Name = "b" });
            //person.Name = "Test";
            //object instance = Activator.CreateInstance(person.GetType(), new object[] { new Person() { Name = "c" }, new Person() { Name = "d" } });
            //Console.WriteLine(":" + person.Name);
            //Console.WriteLine(":" + ((TalkTo)instance).First.Name);
        }
        private Border GetActionButton(string name, string color, BitmapImage icon, RoutedEventHandler handler)
        {
            Border border = (Border)SubWindow.CopyFrom(border_action_vorlage);
            border.Background = ColorHandler.ColorFromHex(color);
            Button btn = (Button)border.Child;
            btn.Click += handler;
            btn.Click += Update;
            StackPanel panel = (StackPanel)btn.Content;
            Image img = (Image)panel.Children[0];
            img.Source = icon;
            Label lbl = (Label)panel.Children[1];
            lbl.Content = name;
            return border;
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            this.Update();
            LoadTabActions();
            Engine.UpdateEverySubWindow();
        }

        //- Update
        public void Update()
        {
            //- Top
            img_top_picture.Source = Person.Image;
            lbl_top_name.Content = Person.Name;
            lbl_top_location.Content = Person.Location.Name;
            lbl_top_origin.Content = Person.Origin.Name;
            lbl_top_ideology.Content = Person.Ideology.Name;
            lbl_top_party.Content = "Parteilos";
            if (Person.Party != null)
                lbl_top_party.Content = Person.Party.Name;
            lbl_top_culture.Content = Person.Culture.Name;
            if (Person.HasProfession())
                lbl_top_employment.Content = Person.MainProfession.Name;

            //- Relation
            if (Engine.CurrentPerson.Knows(Person))
            {
                double this_to_him = Engine.CurrentPerson.GetRelationTo(Person);
                lbl_relation_this_to_he.Content = this_to_him;
                if (this_to_him >= 0)
                {
                    lbl_relation_this_to_he.Foreground = Brushes.Green;
                    lbl_relation_this_to_he.Content = "+" + lbl_relation_this_to_he.Content;
                }
                else
                {
                    lbl_relation_this_to_he.Foreground = Brushes.Red;
                }
            }
            if (Person.Knows(Engine.CurrentPerson))
            {
                double him_to_this = Person.GetRelationTo(Engine.CurrentPerson);
                lbl_relation_he_to_this.Content = him_to_this;
                if (him_to_this >= 0)
                {
                    lbl_relation_he_to_this.Foreground = Brushes.Green;
                    lbl_relation_he_to_this.Content = "+" + lbl_relation_he_to_this.Content;
                }
                else
                {
                    lbl_relation_he_to_this.Foreground = Brushes.Red;
                }
            }

            //- Additional
            lbl_additional_name.Content = Person.Name;
            lbl_additional_origin.Content = Person.Origin.Name;
            lbl_additional_title.Content = "";
            img_additional_follower.Source = Images.IconFollower;
            lbl_additional_follower.Content = Person.Followers.Count + " Anhänger";
            string tt = "Anhänger:\n";
            for (int i = 0; i < Person.Followers.Count; i++)
            {
                tt += "\t" + Person.Followers[i].Name + "\n";
            }
            lbl_additional_follower.ToolTip = tt;
            img_additional_follower.ToolTip = tt;
            //- Titles
            Title highest_title = Person.GetHighestTitle();
            if (highest_title != null)
            {
                lbl_additional_title.Content = highest_title.Name;
                img_additional_title.Source = highest_title.Image;                
            }
            StackPanel parent = panel_additional_titles;
            parent.Children.Clear();
            foreach (Title title in Person.Titles)
            {
                if (title != null)
                {
                    if (title is ArmyLeader leader)
                    {
                        Army army = leader.Person.Army;
                        lbl_additional_army_name.Content = army.Name;
                        lbl_additional_commander_type.Content = leader.Name;
                        parent.Children.Add(panel_additional_commander);
                    }

                    if (title is PartyFounder founder)
                    {
                        Party se = founder.Party;
                        if (title == highest_title)
                            lbl_additional_title.Content = se.ShortName + " " + lbl_additional_title.Content;
                        lbl_additional_party_chef.Content = se.Name;
                        parent.Children.Add(panel_additional_party);
                    }
                }
            }
            //- Profession
            foreach (Profession profession in Person.Professions.Get())
            {
                StackPanel panel;
                if ((panel = profession.GetPersonPanel()).Children.Count > 0)
                    parent.Children.Add(panel);
            }
        }
    }
}
