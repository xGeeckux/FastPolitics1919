using FastPolitics1919.Common;
using FastPolitics1919.Data.Handlers;
using FastPolitics1919.Gfx;
using FastPolitics1919.History.Buildings;
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
    /// Interaktionslogik für ProvinceWindow.xaml
    /// </summary>
    public partial class ProvinceWindow : Window
    {
        public Province Province { get; set; }
        public ProvinceWindow(Province province)
        {
            Province = province;
            InitializeComponent();
            Load();
            Icon = Images.IconCity;
            Title = Province.Name;
        }

        private void Load()
        {
            Update();
            LoadTop();
            LoadTiles();
        }

        private void LoadTop()
        {
            lbl_top_province_name.Content = Province.Name;
            lbl_top_tile_count.Content = Province.Tiles.Count;
            lbl_top_government.Content = "Reichsverwaltung " + Province.Name;

            img_top_owner.Source = Images.FlagREB;
            if (Province.Owner != null)
            {
                lbl_top_government.Content = "Reichsverwaltung " + Province.Owner.Name;
                img_top_owner.Source = Province.Owner.Flag;
            }
        }
        private void LoadTiles()
        {
            panel_tiles.Children.Clear();
            //btn_tile_vorlage_controller.Click += Click;
            foreach (Tile tile in Province.Tiles)
            {
                lbl_tile_vorlage_name.Content = "Umliegende Lanschaft von " + tile.Name;
                lbl_tile_vorlage_citizen_count.Content = tile.CitizenAmount;
                lbl_tile_vorlage_city_name.Content = tile.Name;
                lbl_tile_vorlage_tile_type.Content = "Landschaft";
                if (tile is City)
                    lbl_tile_vorlage_tile_type.Content = "Stadt";
                
                img_tile_vorlage_icon.Source = tile.Icon;
                img_tile_vorlage_money_icon.Source = Images.IconMoney;
                panel_tile_vorlage_buildings.Children.Clear();
                img_tile_vorlage_controller.Source = Images.FlagREB;

                //- OpenCountry
                //btn_tile_vorlage_controller.Tag = tile.CountryController;
                

                if (grid_title_vorlage.Children.Contains(panel_title_vorlage_government_info))
                    grid_title_vorlage.Children.Remove(panel_title_vorlage_government_info);
                if (tile is City city)
                {
                    if (city.Government != null)
                        lbl_title_vorlage_person.Content = city.Government.Cancelor.Name;
                    else
                        lbl_title_vorlage_person.Content = "-";
                    grid_title_vorlage.Children.Add(panel_title_vorlage_government_info);
                }

                if (tile.Controller is Country country)
                    img_tile_vorlage_controller.Source = country.Flag;

                foreach (UniqueBuilding building in tile.UniqueBuildings)
                {
                    if (building == null)
                        continue;

                    Border building_border = new Border();
                    building_border.Background = ColorHandler.ColorFromHex("7FACACAC");
                    building_border.BorderBrush = ColorHandler.ColorFromHex("FF4E555F");
                    building_border.BorderThickness = new Thickness(1);
                    building_border.CornerRadius = new CornerRadius(5);
                    building_border.Padding = new Thickness(2);
                    building_border.HorizontalAlignment = HorizontalAlignment.Center;
                    building_border.Margin = new Thickness(5, 0, 0, 0);
                    building_border.Width = 22;

                    Button btn_building = new Button();
                    btn_building.BorderBrush = null;
                    btn_building.Margin = new Thickness(0);
                    btn_building.Background = null;
                    building_border.Child = btn_building;

                    Image icon = new Image();
                    icon.Margin = new Thickness(-3);
                    btn_building.Content = icon;
                    icon.Source = Images.IconQuestionmark;
                    if (building.Image != null)
                        icon.Source = building.Image;

                    panel_tile_vorlage_buildings.Children.Add(building_border);
                }
                Border outer_border = (Border)SubWindow.CopyFrom(border_tile_vorlage);
                Button btn = (Button)((Border)((StackPanel)((StackPanel)outer_border.Child).Children[0]).Children[((StackPanel)((StackPanel)outer_border.Child).Children[0]).Children.Count - 1]).Child;
                btn.Tag = tile.CountryController;
                btn.Click += Click;
                panel_tiles.Children.Add(outer_border);
            }
        }

        //- Open Controller Country
        private void Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Country country)
                Engine.OpenCountry(country);
        }

        public void Update()
        {

        }
    }
}
