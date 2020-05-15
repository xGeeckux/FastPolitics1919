using FastPolitics1919.Data.Common;
using FastPolitics1919.Data.Common.MapObjects;
using FastPolitics1919.Data.GoogleTabellen;
using FastPolitics1919.Data.Handlers;
using FastPolitics1919.Gfx;
using FastPolitics1919.History.Cultures;
using LibFastPolitics1919.AVL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace FastPolitics1919.Common
{
    public class Map : Canvas
    {
        //- Intern Map
        public HexagonTile[,] Provinces { get; set; }

        //- Length of Provinces
        private int XLength => Provinces.GetLength(0);
        private int YLength => Provinces.GetLength(1);

        //- Selection
        public MapObject CurrentSelected { get; set; }

        //- MapMode
        public MapMode CurrentMapMode { get; set; }

        //- Constructor
        public Map(int x_length, int y_length)
        {
            Provinces = new HexagonTile[x_length, y_length];
        }

        //- After Map is Loaded Constructor
        public void SecoundConstrucotor()
        {
            ChangeMapMode(Engine.Game.EveryMapMode.Find(0));
        }

        //- Change Mapmode
        public void ChangeMapMode(MapMode mapmode)
        {
            if (mapmode == null)
                return;
            if (CurrentMapMode != null)
                CurrentMapMode.OnDeselection();
            CurrentMapMode = mapmode;
            Log.Write("Mapmode changed to: " + CurrentMapMode.Name);
            CurrentMapMode.OnSelection();
        }
        public void Update()
        {
            ChangeMapMode(CurrentMapMode);
        }

        //- Change Selection
        public void SetCurrentSelected(MapObject obj)
        {
            if (obj == null)
                return;
            if (CurrentSelected == obj)
            {
                Log.Write("Deselected");
                if (CurrentSelected is Unit)
                    Engine.GameInterface.CloseUnit();
                CurrentSelected = null;
            }
            else
            {
                Log.Write("New Selection: " + obj.Name);
                CurrentSelected = obj;
                if (CurrentSelected is Unit unit)
                    Engine.OpenUnit(unit);
            }
            foreach (HexagonTile province in Provinces)
                if (province != null)
                    province.Update();
        }

        //- Calc own Height and Width
        private void CalcHeightAndWidth()
        {
            double calc = YLength * HexagonTile.YCor + (HexagonTile.YCor / 2);
            Height = calc;
            calc = XLength * HexagonTile.XCor + 31;
            Width = calc;
        }

        //- Set Hexagon on Top
        public void SetOnTop(HexagonTile tile)
        {
            Children.Remove(tile);
            Children.Add(tile);
        }

        //- Fill the Map with Provinces
        private void FillMap()
        {
            int id_index = 0;
            for (int y = 0; y < YLength; y++)
            {
                for (int x = 0; x < XLength; x++)
                {
                    Provinces[x, y] = new HexagonTile(id_index, x, y);
                    Children.Add(Provinces[x, y]);
                    id_index++;
                }
            }
        }
        
        //- Centre of Polygons
        private void FindCentre(List<HexagonTile> hex_provinces)
        {
            int p1_left = XLength, p1_top = YLength, p2_left = 0, p2_top = 0;
            foreach (HexagonTile province in hex_provinces)
            {
                if (province.X < p1_left)
                    p1_left = province.X;
                if (province.X > p2_left)
                    p2_left = province.Y;
                if (province.Y < p1_top)
                    p1_top = province.X;
                if (province.Y > p2_top)
                    p2_top = province.Y;
            }
            int d_left = Math.Abs(p1_left - p2_left);
            int d_top = Math.Abs(p1_top - p2_top);
            p1_left += d_left;
            p1_top += d_top;

            Grid grid = new Grid();
            grid.Height = 50;
            grid.Width = 50;
            grid.Background = Brushes.Red;
            this.Children.Add(grid);
            SetTop(grid, 50 * p1_top + grid.Width / 2);
            SetLeft(grid, 101 * p1_left + grid.Width / 2);
        }

        //- Tile Generator
        public class TileGenerator
        {
            private static System.Drawing.Bitmap Image { get; set; }
            private static Map Map { get; set; }

            private class ProvincePixel
            {
                public int ID { get; set; }
                public List<CityPixel> Cities { get; set; }
                public string Color { get; set; }

                public class CityPixel
                {
                    public int ID { get; set; }

                    public int X { get; set; }
                    public int Y { get; set; }
                }
            }

            private static List<ProvincePixel> Cities = new List<ProvincePixel>();
            private static List<string> ActiveColors = new List<string>();
            private static void LoadMap()
            {
                int city_id_index = 0;
                int province_id_index = 0;
                for (int y = 0; y < Map.YLength; y++)
                {
                    for (int x = 0; x < Map.XLength; x++)
                    {
                        System.Drawing.Color pixel = Image.GetPixel(x, y);
                        string color = pixel.R + "-" + pixel.G + "-" + pixel.B;
                        if (color != "0-0-0")
                        {
                            if (!ActiveColors.Contains(color))
                            {
                                ActiveColors.Add(color);
                                ProvincePixel pp = new ProvincePixel
                                {
                                    ID = province_id_index,
                                    Cities = new List<ProvincePixel.CityPixel>(),
                                    Color = color
                                };
                                ProvincePixel.CityPixel cp = new ProvincePixel.CityPixel
                                {
                                    ID = city_id_index,
                                    X = x,
                                    Y = y
                                };
                                pp.Cities.Add(cp);
                                Cities.Add(pp);
                                province_id_index++;
                            }
                            else
                            {
                                for (int i = 0; i < ActiveColors.Count; i++)
                                {
                                    if (ActiveColors[i] == color)
                                    {
                                        ProvincePixel.CityPixel cp = new ProvincePixel.CityPixel
                                        {
                                            ID = city_id_index,
                                            X = x,
                                            Y = y
                                        };
                                        Cities[i].Cities.Add(cp);
                                    }
                                }
                            }
                            Map.Provinces[x, y] = new HexagonTile(city_id_index, x, y);
                            Map.Provinces[x, y].Color = color;
                            Map.Children.Add(Map.Provinces[x, y]);
                            city_id_index++;
                        }
                    }
                }
            }

            public static Map CreateTiles()
            {
                LoadImage();
                InitMap();
                LoadMap();

                CreateProvinces();
                CreateCities();

                InitCities();

                Map.CalcHeightAndWidth();
                DragHandler.MakeDragable(Map, true);

                return Map;
            }

            public static void FinishLoading()
            {
                Map.SecoundConstrucotor();
            }

            //- Random
            private static Random Random = new Random();

            private static void CreateProvinces()
            {
                foreach (ProvincePixel pixel in Cities)
                {
                    if(Engine.Game.FindProvince(pixel.ID) == null)
                    {
                        Province province = new Province();
                        province.ID = pixel.ID;
                        province.Name = "Province " + (province.ID);
                        province.Owner = Engine.Game.FindCountry(0);
                        Engine.Game.Provinces.Add(province, province.ID);
                    }
                }

                GoogleSheet sheet = new GoogleSheet();
                sheet.SetSheetUrl("1ZjzbXTa93K7G3zTYgRdyk7jA7aS0i4Mo_myblGjOE18");
                sheet.SetSheetTab("Provinces");
                List<GoogleCell[]> cells = sheet.ReadCells("A2:B");
                
                foreach (GoogleCell[] cell in cells)
                {
                    if (cell.Length < 2 || cell[0] == null || cell[1] == null)
                        continue;
                    int id = Convert.ToInt32(cell[0].Content.ToString());
                    string name = cell[1].Content.ToString();
                    Province local = Engine.Game.FindProvince(id);
                    if (local != null)
                    {
                        local.Name = name;
                    }
                }
            }
            private static void CreateCities()
            {
                int j = 1;
                foreach (ProvincePixel pixel in Cities)
                {
                    foreach (ProvincePixel.CityPixel city in pixel.Cities)
                    {
                        if (Engine.Game.FindTile(city.ID) == null)
                        {
                            if (j == 8)
                                j = 1;
                            Tile local = new Landscape();
                            local.ID = city.ID;
                            local.Name = "S " + (local.ID);
                            local.Owner = Engine.Game.FindProvince(pixel.ID);
                            local.Culture = Engine.Game.FindCulture(Cultures.Deutsch);
                            local.BackGroundImage = Images.FromPath(Images.map_tiles + "tile_landschaft_" + j);
                            int n = Random.Next(1, 4 + 1) + Random.Next(1, 6+ 1);
                            for (int i = 0; i < n; i++)
                            {
                                local.CreatePeasant();
                            }
                            Engine.Game.Tiles.Add(local, local.ID);
                            j++;
                        }
                    }
                }

                GoogleSheet sheet = new GoogleSheet();
                sheet.SetSheetUrl("1ZjzbXTa93K7G3zTYgRdyk7jA7aS0i4Mo_myblGjOE18");
                sheet.SetSheetTab("Cities");
                List<GoogleCell[]> cells = sheet.ReadCells("A2:C");

                int k = 1;
                foreach (GoogleCell[] cell in cells)
                {
                    if (cell.Length < 2 || cell[0] == null || cell[1] == null)
                        continue;

                    int id = Convert.ToInt32(cell[0].Content.ToString());
                    string name = cell[1].Content.ToString();
                    bool is_city = false;
                    if (cell.Length == 3)
                        is_city = Convert.ToBoolean(cell[2].Content.ToString());

                    Tile local = Engine.Game.FindTile(id);
                    if (local != null)
                    {
                        local.Name = name;
                        //- Debug pls fix later
                        if (is_city)
                        {
                            if (k == 5)
                                k = 1;
                            City city = new City();
                            Engine.CopyProperties(local, city);
                            city.Founder = city.CountryOwner;
                            city.BackGroundImage = Images.FromPath(Images.map_tiles + "tile_city_" + k);
                            int n = 2 + Random.Next(1, 10 + 1) + Random.Next(1, 10 + 1);
                            for (int i = 0; i < n; i++)
                                city.CreateCitizen();
                            Engine.Game.Tiles.Remove(city.ID);
                            Engine.Game.Tiles.Add(city, city.ID);
                            k++;
                        }
                    }
                }
            }

            private static void InitCities()
            {
                foreach (HexagonTile tile in Map.Provinces)
                {
                    if (tile != null)
                    {
                        tile.Tile.Controller = tile.Tile.CountryOwner;
                    }
                }
            }

            private static void LoadImage()
            {
                Image = new System.Drawing.Bitmap(Environment.CurrentDirectory + @"\gfx\Map2.png");

            }
            private static void InitMap()
            {
                Map = new Map(Image.Width, Image.Height);
            }
        }
    }
}
