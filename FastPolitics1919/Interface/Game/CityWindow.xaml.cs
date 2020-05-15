using FastPolitics1919.Common;
using FastPolitics1919.Data.Common;
using FastPolitics1919.Data.Common.BuildProcesses;
using FastPolitics1919.Data.Handlers;
using FastPolitics1919.Gfx;
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
    public partial class CityWindow : Window
    {
        //- Variables
        public Tile Tile { get; set; }

        //- Constructor
        public CityWindow(Tile tile)
        {
            Tile = tile;
            Icon = Images.IconDevelopment;
            InitializeComponent();
            Load();
        }

        //- Load
        private void Load()
        {
            Update();
            
            LoadTabs();

            //- Debug
            //Label copy_lbl = (Label)SubWindow.CopyFrom(debug_point);
            //debug_list.Children.Clear();
            //debug_point = null;
            //foreach (Ideology.Amount ideology in Tile.Ideologies)
            //{
            //    Label lbl = (Label)SubWindow.CopyFrom(copy_lbl);
            //    lbl.Content = ideology.Ideology.Name + ": " + ideology.Proportion;
            //    debug_list.Children.Add(lbl);
            //}
        }

        private void LoadTabs()
        {
            LoadTabCommon();
            LoadTabParlament();
            LoadTabPerson();
            DeClips();
        }
        private void LoadTabCommon()
        {
            img_common_header.Source = Images.IconTabCommon;
            img_common_bg.Source = Tile.BackgroundImage;
            lbl_common_city_name.Content = Tile.Name;
            lbl_common_province_name.Content = Tile.Owner.Name;
            img_common_city.Source = Tile.Icon;
            img_common_capital.Source = Images.IconCapital;
            StackPanel parent_capital = (StackPanel)lbl_common_city_name.Parent;
            if(Tile.ID != Tile.Owner.Owner.CapitalID)
            {
                parent_capital.Children.Remove(border_common_capital);
            }
        }
        private void LoadTabParlament()
        {
            if (Tile is Landscape)
            {
                tab_control.Items.Remove(tab_parlament);
            }
            img_parlament_header.Source = Images.IconTabParlament;
            img_parlament_bg.Source = Images.IconBackground2;

            Border vorlage = (Border)SubWindow.CopyFrom(border_parlament_vorlage);
            panel_parlament_head.Children.Clear();
            border_parlament_vorlage = null;
            panel_parlament_body.Children.Clear();
            lbl_government_person.Content = Tile.Persons.Count + " Personen";
            if (Tile is City city)
            {
                if (city.Government == null)
                    return;
                lbl_parlament_ruling_party.Content = city.Government.RulingParties[0].ShortName;
                lbl_government_last_election.Content = city.Government.LastElection.ToString();
                lbl_government_next_election.Content = city.Government.NextElection.ToString();
                lbl_government_party.Content = city.Government.RegisteredParties.Count + " Parteien";
                if (city.Government.Cancelor != null)
                {
                    Border canelor = (Border)SubWindow.CopyFrom(vorlage);
                    Image img = (Image)((Button)(canelor.Child)).Content;
                    img.Source = city.Government.Cancelor.Image;
                    canelor.ToolTip = city.Government.Cancelor.Name + " (" + city.Government.Cancelor.Party.ShortName + ")";
                    panel_parlament_head.Children.Add(canelor);
                }
                if (city.Government.Seats.Count > 0)
                {
                    int count = city.Government.Seats.Count;
                    StackPanel hor_panel = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center };
                    for (int i = 0; i < city.Government.Seats.Count; i++)
                    {
                        Border seat = (Border)SubWindow.CopyFrom(vorlage);
                        Button btn = (Button)(seat.Child);
                        btn.Click += city.Government.Seats[i].Person.PersonClick;
                        Image img = (Image)btn.Content;
                        img.Source = city.Government.Seats[i].Person.Image;
                        seat.ToolTip = city.Government.Seats[i].Person.Name + " (" + city.Government.Seats[i].Party.ShortName + ")";
                        seat.Background = ColorHandler.ColorFromRGB(city.Government.Seats[i].Party.Color);
                        hor_panel.Children.Add(seat);
                        if (hor_panel.Children.Count == 12)
                        {
                            panel_parlament_body.Children.Add(hor_panel);
                            hor_panel = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center };
                        }
                    }
                    if (hor_panel.Children.Count != 0)
                    {
                        panel_parlament_body.Children.Add(hor_panel);
                    }
                }
            }
        }

        private void DeClips()
        {
            tab_control.Items.Remove(tab_party);
        }
        private void LoadTabPerson()
        {
            if (!tab_control.Items.Contains(tab_person))
                tab_control.Items.Add(tab_person);
            img_person_header.Source = Images.IconMalePerson;
            img_person_bg.Source = Images.IconBackground3;

            lbl_persons_count.Content = Tile.Persons.Count;
            
            panel_persons_list.Children.Clear();
            if (Engine.CurrentPerson.Location == Tile)
                AddPerson(Engine.CurrentPerson);
            for (int i = 0; i < Tile.Persons.Count; i++)
            {
                if (Tile.Persons[i] == Engine.CurrentPerson)
                    continue;
                AddPerson(Tile.Persons[i]);
            }
        }
        private void AddPerson(Person person)
        {
            img_person_picture.Source = person.Image;
            lbl_person_name.Content = person.Name;
            lbl_person_party.Content = "Parteilos";
            if (person.Party != null)
            lbl_person_party.Content = person.Party.Name;
            lbl_person_age.Content = "";
            lbl_person_this_to_him.Content = "-";
            lbl_person_this_to_him.Foreground = Brushes.White;
            lbl_person_this_to_him.ToolTip = "Unsere Meinung über ihn";
            lbl_person_him_to_this.Content = "-";
            lbl_person_him_to_this.Foreground = Brushes.White;
            lbl_person_him_to_this.ToolTip = "Seine Meinung über uns";
            if (Engine.CurrentPerson.Knows(person))
            {
                double this_to_him = Engine.CurrentPerson.GetRelationTo(person);
                lbl_person_this_to_him.Content = this_to_him;
                if (this_to_him >= 0)
                {
                    lbl_person_this_to_him.Foreground = Brushes.Green;
                    lbl_person_this_to_him.Content = "+" + lbl_person_this_to_him.Content;
                }
                else
                    lbl_person_this_to_him.Foreground = Brushes.Red;
            }
            if (person.Knows(Engine.CurrentPerson))
            {
                double him_to_this = person.GetRelationTo(Engine.CurrentPerson);
                lbl_person_him_to_this.Content = him_to_this;
                if (him_to_this >= 0)
                {
                    lbl_person_him_to_this.Foreground = Brushes.Green;
                    lbl_person_him_to_this.Content = "+" + lbl_person_him_to_this.Content;
                }
                else
                    lbl_person_him_to_this.Foreground = Brushes.Red;
            }
            Border border = (Border)SubWindow.CopyFrom(border_person_vorlage);
            border.MouseUp += person.PersonClick;
            panel_persons_list.Children.Add(border);
        }
        //- Txt Changes to Filter
        private void Txt_persons_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            panel_persons_list.Children.Clear();
            string txt = txt_persons_search.Text;
            if (txt == "")
            {
                LoadTabPerson();
                return;
            }
            for (int i = 0; i < Tile.Persons.Count; i++)
            {
                try
                {
                    string sub = Tile.Persons[i].Name.Substring(0, txt.Length);
                    if (sub == txt)
                    {
                        AddPerson(Tile.Persons[i]);
                        return;
                    }
                    string[] splitted = Tile.Persons[i].Name.Split(' ');
                    for (int j = 0; j < splitted.Length; j++)
                    {
                        if (txt == splitted[j].Substring(0, txt.Length))
                            AddPerson(Tile.Persons[i]);
                    }
                }
                catch (Exception) { }
            }
        }

        private void LoadTabParty()
        {
            tab_control.Items.Add(tab_party);
            img_party_header.Source = Images.IconIdeology;
            img_party_bg.Source = Images.IconBackground4;
            if (Tile is City city)
            {
                panel_party_list.Children.Clear();
                //- Country
                if (city.CountryOwner != null && city.CountryOwner.Government != null)
                {
                    foreach (Party party in city.CountryOwner.Government.RegisteredParties)
                    {
                        AddParty(party, city.CountryOwner.Name);
                    }
                }
                //- Province
                if (city.Owner != null && city.Owner.Government != null)
                {
                    foreach (Party party in city.Owner.Government.RegisteredParties)
                    {
                        AddParty(party, city.Owner.Name);
                    }
                }
                //- City
                if (city.Government != null)
                {
                    foreach (Party party in city.Government.RegisteredParties)
                    {
                        AddParty(party, city.Name);
                    }
                }
            }

        }
        private void AddParty(Party party, string from)
        {
            lbl_party_name.Content = party.Name;
            lbl_party_founder.Content = "<Unbekannt>";
            if (party.Founder != null)
                lbl_party_founder.Content = party.Founder.Name;
            lbl_party_from.Content = from;
            lbl_party_members.Content = party.MemberCount + " Members";
            lbl_party_politicians.Content = party.GetPossiblePersons().Length + " Politiker";
            Border border = (Border)SubWindow.CopyFrom(border_party_vorlage);
            panel_party_list.Children.Add(border);
        }

        //- Update
        public void Update()
        {
            //- City Founding
            Grid parent = grid_center;
            parent.Children.Remove(gui_grid_special_city);
            parent.Children.Remove(gui_grid_special_landscape);
            parent.Children.Remove(gui_grid_special_founding);

            if (Tile is City city2)
            {
                parent.Children.Add(gui_grid_special_city);

                //- Unique Buildings..
                Border building_border = (Border)SubWindow.CopyFrom(border_special_unique_building);
                panel_special_unique_buildings.Children.Clear();

                lbl_special_unique_building_name.Content = "";
                lbl_special_unique_building_desc.Text = "";

                for (int i = 0; i < city2.UniqueBuildings.Length; i++)
                {
                    Border border = (Border)SubWindow.CopyFrom(building_border);
                    border.Tag = city2.UniqueBuildings[i];
                    Grid g = (Grid)border.Child;
                    Button b = (Button)g.Children[0];
                    Image img = (Image)b.Content;
                    img.Source = Images.IconEmptyBuilding;
                    border.MouseEnter += BuildingHover;
                    if(city2.UniqueBuildings[i] != null)
                    {
                        img.Source = city2.UniqueBuildings[i].Image;
                        border.ToolTip = city2.UniqueBuildings[i].Name;
                    }
                    panel_special_unique_buildings.Children.Add(border);
                }

                //- Military Buildings..
                Border military_border = (Border)SubWindow.CopyFrom(border_special_military_building);
                panel_special_military_buildings.Children.Clear();

                for (int i = 0; i < city2.MilitaryBuildings.Length; i++)
                {
                    Border border = (Border)SubWindow.CopyFrom(military_border);
                    border.Tag = city2.MilitaryBuildings[i];
                    Border b = (Border)border.Child;
                    Image img = (Image)b.Child;
                    img.Source = Images.IconEmptyBuilding;
                    border.MouseEnter += BuildingHover;
                    if (city2.MilitaryBuildings[i] != null)
                    {
                        img.Source = city2.MilitaryBuildings[i].Image;
                        border.ToolTip = city2.MilitaryBuildings[i].Name;
                    }
                    panel_special_military_buildings.Children.Add(border);
                }

                //- Government Building
                img_special_government_building.Source = Images.IconEmptyBuilding;
                img_special_government_building_icon.Source = null;
                lbl_special_government_building_name.Content = "";
                lbl_special_government_building_type.Content = "";
                if (city2.GovernmentBuilding != null)
                {
                    img_special_government_building.Source = city2.GovernmentBuilding.Image;
                    img_special_government_building_icon.Source = Images.IconBuilding;
                    lbl_special_government_building_name.Content = city2.GovernmentBuilding.Name;
                    lbl_special_government_building_type.Content = "Regierungsgebäude";
                }
                progessbar_special_progress.Value = 100;
            }
            //- Upgrading
            if (Tile is Landscape && !Tile.Upgrading)
                parent.Children.Add(gui_grid_special_landscape);
            else if (Tile.MyProcess != null)
            {
                parent.Children.Add(gui_grid_special_founding);
                lbl_special_founding_duration.Content = Tile.MyProcess.RemainingRounds.Number;

                StackPanel panel = Tile.GetPropertyPanel(Modifiers.LocalCityFoundModifier);
                panel.Background = Brushes.Black;
                lbl_special_founding_duration.ToolTip = Tile.GetTooltip(Modifiers.LocalCityFoundModifier, Tile.BaseProcessRound, Tile.ProcessRound);

                lbl_special_founding_type.Content = "Stadt";
                if (!(Engine.CurrentPerson == null || Engine.CurrentPerson == Tile.Founder))
                {
                    StackPanel p = (StackPanel)panel_special_founding_name.Parent;
                    p.Children.Remove(panel_special_founding_name);
                }
                if (Tile.Founder != null)
                    lbl_special_founding_founder.Content = Tile.Founder.Name;
                else
                    lbl_special_founding_founder.Content = "Keiner";
                txt_special_founding_cityname.Text = Tile.Name;
                progessbar_special_progress.Value = Tile.MyProcess.Status * 100;
            }

            Title = Tile.Name;
            if (Tile is City)
            {
                Title += " (Stadt)";
                lbl_info_city_category.Content = "Stadt";
            }
            if (Tile is Landscape)
            {
                Title += " (Lanschaft)";
                lbl_info_city_category.Content = "Landschaft";
            }

            //- Top
            if(Tile is City city && city.Government != null)
            {
                lbl_top_cancelor.Content = city.Government.Cancelor.Name;
                lbl_top_cancelor.ToolTip = "[" + city.Government.Cancelor.ID + "]";
                lbl_top_ruling_party.Content = city.Government.RulingParties[0].ShortName;
            }

            //- Info
            lbl_info_owner.Content = Tile.Owner.Name;
            btn_info_owner.Click += OwnerClick;
            lbl_info_owner_owner.Content = Tile.Owner.Owner.Name;
            img_info_owner_owner.Source = Tile.Owner.Owner.Flag;
            btn_info_owner_owner.Click += OwnerOwnerClick;
            lbl_info_dichte.Content = Math.Round(Tile.Dichte,1) + " Bev./km²";
            lbl_info_culture.Content = Tile.Culture.Name;
            if (Tile.Founder != null)
                lbl_info_founder.Content = Tile.Founder.Name;
            if (Tile.Founder != null && Tile.Founder is Country)
                lbl_info_founder.Content += " (Staat)";

            //- General
            lbl_general_citizen_count.Content = Tile.CitizenCount + " Citizens";
            string line = "Citizen Ideology:\n";
            for (int i = 0; i < Tile.CitizenCount; i++)
            {
                line += Tile.Citizens[i].Name + " => " + Tile.Citizens[i].Ideology.Name + "\n";
            }
            lbl_general_citizen_count.ToolTip = line;
            lbl_general_citizen_amount.Content = TextHandler.ToFormatNumber(Tile.CitizenAmount) + " Bürger";
            lbl_general_person_count.Content = Tile.Persons.Count + " Personen";
            line = "Persons:\n";
            for (int i = 0; i < Tile.Persons.Count; i++)
            {
                if (Tile.Persons[i].Ideology != null)
                    line += Tile.Persons[i].Name + " (" + Tile.Persons[i].Ideology.Name + ")";
                else
                    line += Tile.Persons[i].Name;
                if (Tile.Persons[i] is Player)
                    line += " [Ist Spieler]";
                line += "\n";
            }
            lbl_general_person_count.ToolTip = line;
            lbl_general_building_count.Content = Tile.BuildingCount + " Gebäude";
            lbl_general_military_building_count.Content = Tile.MilitaryBuildingCount + " Gebäude";
            lbl_general_building_processes_count.Content = Tile.BuildingProcesses + " Projekte";

            //- Tabs
            LoadTabPerson();
        }


        //- Click on (Owner) Province-Name
        private void OwnerClick(object sender, RoutedEventArgs e)
        {
            Engine.OpenProvince(Tile.Owner);
        }
        private void OwnerOwnerClick(object sender, RoutedEventArgs e)
        {
            Engine.OpenCountry(Tile.CountryOwner);
        }

        private void BuildingHover(object sender, MouseEventArgs e)
        {
            Border border = (Border)sender;
            Building building = (Building)border.Tag;
            lbl_special_unique_building_name.Content = "Leerer Gebäude-Slot";
            lbl_special_unique_building_desc.Text = "Hier könnte ein weiteres Gebäude gebaut werden.";
            if (building != null)
            {
                lbl_special_unique_building_name.Content = building.Name;
                lbl_special_unique_building_desc.Text = building.Description;
            }
            lbl_special_unique_building_desc.ToolTip = lbl_special_unique_building_desc.Text;
        }

        //- Txt changes Province Name
        private void Txt_special_founding_cityname_TextChanged(object sender, TextChangedEventArgs e)
        {
            Tile.Name = txt_special_founding_cityname.Text;
        }

        //- Found City
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new FoundCityProcess(Tile);
            Update();
        }

        //- Click Persons
        private void Btn_government_person_list_Click(object sender, RoutedEventArgs e)
        {
            LoadTabPerson();
            for (int i = 0; i < tab_control.Items.Count; i++)
                if (tab_control.Items[i] == tab_person)
                {
                    tab_control.SelectedIndex = i;
                    return;
                }
        }
        //- Click Party
        private bool PartyLoad = false;
        private void Lbl_government_party_list_Click(object sender, RoutedEventArgs e)
        {
            if (!PartyLoad)
            {
                LoadTabParty();
                tab_control.SelectedIndex = tab_control.Items.Count - 1;
                PartyLoad = true;
            }
        }
    }
}
