using FastPolitics1919.Common;
using FastPolitics1919.Data.Handlers;
using FastPolitics1919.Gfx;
using FastPolitics1919.History.Orders;
using FastPolitics1919.Interface;
using FastPolitics1919.Interface.Game;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FastPolitics1919.Data.Common.MapObjects
{
    public class HexagonTile : Grid
    {
        //- Unique ID for Province compare
        public int ID { get; set; }
        public Tile Tile => Engine.Game.FindTile(ID);
        public bool HasCity => Tile == null ? false : true;

        //- Local Map Vars
        private Map DirectParent => (Map)this.Parent;
        public Polygon Polygon { get; set; }
        public string Color { get; set; }
        private SolidColorBrush DefaultColor
        {
            get
            {
                return ColorHandler.ColorFromRGB(Color);
            }
        }

        //- Coordinates
        public int X { get; set; }
        public int Y { get; set; }

        //- Constructor
        public HexagonTile(int id, int x, int y)
        {
            ID = id;

            Load();
            SetCor(x, y);
            Update();
        }

        //- Secound Constructor
        private void Load()
        {
            //- Init Polygon
            Children.Add(Polygon = new Polygon { Fill = DefaultColor, Points = 
                new PointCollection { new Point(30, 0), new Point(90, 0), new Point(120, 50), new Point(90, 100), new Point(30, 100), new Point(0, 50) } });

            //- Eventhandlers
            MouseEnter += Hover;
            MouseLeave += Leave;
            MouseLeftButtonUp += Click;
            MouseRightButtonUp += RightClick;
        }

        //- Update
        public void Update()
        {

        }

        //- Methodes for Mapmodes
        public void ResetEveryThing()
        {
            SubWindowType = null;
            Children.Clear();
            Children.Add(Polygon);
        }

        #region MapModes Methodes
        private Image BackGround = new Image
        {
            Source = Images.TileLandschaft,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,
            Height = 100,
            Width = 120
        };
        public void ToggleGraphics(double opactiy)
        {
            #region Debug
            BackGround.Opacity = opactiy;
            if (Tile.BackGroundImage != null)
                BackGround.Source = Tile.BackGroundImage;
            if (!Children.Contains(BackGround))
                Children.Add(BackGround);
            #endregion
        }

        public void TogglePlayerIcon()
        {
            Image img = new Image
            {
                Height = 40,
                Margin = new Thickness(0, 5, 15, 0),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Right,
                Source = Images.IconPlayer,
                
            };
            if (Engine.CurrentPerson != null && Engine.CurrentPerson.LocationID == Tile.ID)
            {
                Children.Add(img);
                img.ToolTip = Engine.CurrentPerson.Name;
            }
        }

        public void ToggleTileName()
        {
            Label lbl = new Label
            {
                Foreground = Brushes.White,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Content = Tile.Name //+ " (" + X + "," + Y + ")"
            };
            Children.Add(lbl);
        }

        public void ToggleUnit()
        {
            if (Children.Contains(UnitGrid))
                Children.Remove(UnitGrid);
            UnitGrid = new Grid
            {
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(0, 0, 0, 10)
            };
            Children.Add(UnitGrid);

            //- Units
            for (int i = 0; i < Tile.Units.Count; i++)
            {
                if (i == 5)
                    break;
                Border border = CreateUnit(Tile.Units[i]);
                border.VerticalAlignment = VerticalAlignment.Bottom;
                border.MouseEnter += CounterEnter;
                border.Margin = new Thickness(0, 0, 0, i * 15);
                UnitGrid.Children.Add(border);
            }
        }
        private Grid UnitGrid { get; set; }
        private bool OnCounterHover { get; set; }
        private void CounterEnter(object sender, MouseEventArgs e)
        {
            OnCounterHover = true;
            Engine.Map.SetOnTop(this);
            UnitGrid.Children.Clear();
            if (Tile.HasBattle)
            {
                Battle battle = Tile.Battles[0];
                StackPanel hor_panel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Background = ColorHandler.ColorFromARGB(100, 100, 100, 100),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                hor_panel.MouseLeave += Leave;
                UnitGrid.Children.Add(hor_panel);

                StackPanel attacker_panel = CreatePanel(battle.Attackers);
                hor_panel.Children.Add(attacker_panel);

                Image img = new Image();
                img.Source = Images.IconQuestionmark;
                img.Width = 20;
                hor_panel.Children.Add(img);

                StackPanel defender_panel = CreatePanel(battle.Defenders);
                hor_panel.Children.Add(defender_panel);

                int counter_width = 50;
                double left_shift = counter_width + img.Width + counter_width;
                left_shift -= 100;
                left_shift /= 2;
                hor_panel.Margin = new Thickness(-left_shift, 0, 0, 0);
            }
            else
            {
                StackPanel panel = new StackPanel();
                panel.Background = Brushes.Gray;
                panel.VerticalAlignment = VerticalAlignment.Center;
                panel.HorizontalAlignment = HorizontalAlignment.Center;
                panel.MouseLeave += Leave;
                UnitGrid.Children.Add(panel);

                //- Units
                double height_count = 0;
                for (int i = 0; i < Tile.Units.Count; i++)
                {
                    Border border = CreateUnit(Tile.Units[i]);
                    border.Margin = new Thickness(4);
                    height_count += border.Height + 8;
                    panel.Children.Add(border);
                }
                if (height_count > 100)
                {
                    double height = height_count - 100;
                    height /= 2;
                    UnitGrid.Margin = new Thickness(0, -height, 0, 0);
                }
            }
        }
        private Border CreateUnit(Unit unit)
        {
            UnitCounter counter = new UnitCounter();
            //counter.lbl_unit_type.Content = unit.Symbol;
            counter.img_unit_size.Source = unit.Symbol;
            counter.img_unit_counter.Source = Images.IconQuestionmark;
            if (unit.Color != null)
                counter.border_unit.Background = ColorHandler.ColorFromRGB(unit.Color);
            
            counter.img_unit_counter.Source = unit.IconCounter;

            string tt = unit.Name + "\n";
            if (unit.Owner != null)
                tt += "Besitzer von " + unit.Owner.Name + "\n";
            if (unit.Commander != null)
                tt += "Unter kontrolle von " + unit.Commander.Name;
            counter.border_unit.ToolTip = tt;

            Border border = (Border)SubWindow.CopyFrom(counter.border_unit);
            unit.BorderParent = border;
            border.Tag = unit;
            border.MouseUp += UnitCounterClick;
            return border;
        }
        private void UnitCounterClick(object sender, MouseButtonEventArgs e)
        {
            Unit unit = (Unit)((Border)sender).Tag;
            Engine.Map.SetCurrentSelected(unit);
        }
        private StackPanel CreatePanel(List<Unit> units)
        {
            StackPanel panel = new StackPanel();
            panel.Background = Brushes.Gray;
            panel.VerticalAlignment = VerticalAlignment.Center;
            panel.HorizontalAlignment = HorizontalAlignment.Center;
            panel.MouseLeave += Leave;

            //- Units
            for (int i = 0; i < units.Count; i++)
            {
                Border border = CreateUnit(units[i]);
                border.Margin = new Thickness(4);
                panel.Children.Add(border);
            }

            return panel;
        }
        private void LoadUnitActionMenu()
        {
            ContextMenu = new ContextMenu();

            MenuItem teleport_to = new MenuItem();
            teleport_to.Header = "Teleportiere Einheit hier hin";
            teleport_to.Click += UnitTeleportClick;
            ContextMenu.Items.Add(teleport_to);

            MenuItem move_to = new MenuItem();
            move_to.Header = "Einheit hier hin befehligen";
            move_to.Click += UnitMoveClick;
            ContextMenu.Items.Add(move_to);
        }
        
        private void UnitTeleportClick(object sender, RoutedEventArgs e)
        {
            if (Engine.Map.CurrentSelected != null && Engine.Map.CurrentSelected is Unit unit)
            {
                unit.MoveTo(Tile);
                Engine.Map.Update();
            }
        }
        private void UnitMoveClick(object sender, RoutedEventArgs e)
        {
            if (Engine.Map.CurrentSelected != null && Engine.Map.CurrentSelected is Unit unit)
            {
                Tile[] list = unit.Location.FindPath(Tile);
                foreach (Tile tile in list)
                    unit.Orders.Add(new MoveOrder(unit, tile.ID));
                Engine.OpenUnit(unit);
            }
        }

        public void ToggleCityColor()
        {
            Polygon.Fill = ColorHandler.ColorFromRGB("0-0-0");
        }

        public void ToggleProvinceColor()
        {
            if (Tile.Owner != null)
                Polygon.Fill = ColorHandler.ColorFromRGB(Color);
        }

        public void ToggleCountryColor()
        {
            if (Tile.CountryOwner != null)
                Polygon.Fill = ColorHandler.ColorFromRGB(Tile.CountryOwner.RGBColor);
        }

        public void ToggleBlackColor()
        {
            Polygon.Fill = ColorHandler.ColorFromRGB("10-10-10");
        }

        public void ToggleControllerColor()
        {
            if (Tile.Controller != Tile.CountryOwner)
            {
                SolidColorBrush brush = ColorHandler.ColorFromRGB("10-10-10");
                if (Tile.CountryController != null)
                    brush = ColorHandler.ColorFromRGB(Tile.CountryController.RGBColor);
                Polygon.Fill = Hatch(brush);
            }
        }

        public void ToggleTest()
        {

        }
        private VisualBrush Hatch(SolidColorBrush brush)
        {
            VisualBrush VB = new VisualBrush();
            VB.Viewport = new Rect(0, 0, 10, 10);
            VB.ViewportUnits = BrushMappingMode.Absolute;
            VB.TileMode = TileMode.Tile;

            Canvas c = new Canvas();
            Grid rec = new Grid();
            rec.Background = Polygon.Fill;
            rec.Height = 12;
            rec.Width = 12;
            c.Children.Add(rec);
            Line L = new Line();
            L.X1 = 0;
            L.Y1 = 12;
            L.X2 = 12;
            L.Y2 = 0;
            L.Stroke = brush;
            L.StrokeThickness = 2;
            c.Children.Add(L);

            VB.Visual = c;
            return VB;
        }

        public void ToggleOpenTile()
        {
            SubWindowType = typeof(CitySubWindow);
        }

        public void ToggleOpenProvince()
        {
            SubWindowType = typeof(ProvinceSubWindow);
        }
        #endregion

        //- Hover
        #region Hover
        public void Hover(object sender, MouseEventArgs e)
        {
            Polygon.Fill = Brushes.LightGray;
            Engine.Map.CurrentMapMode.OnHover(Tile);

            if (Engine.Map.CurrentSelected != null && Engine.Map.CurrentSelected is Unit)
                LoadUnitActionMenu();
        }
        public void Leave(object sender, MouseEventArgs e)
        {
            ResetEveryThing();
            Engine.Map.CurrentMapMode.ForEvery(Tile);
            Engine.Map.CurrentMapMode.OnHoverLeave(Tile);
            OnCounterHover = false;
        }
        #endregion

        //- Click
        public Type SubWindowType { get; set; }
        public void Click(object sender, MouseButtonEventArgs e)
        {
            if (SubWindowType != null && !OnCounterHover)
            {
                if (SubWindowType == typeof(ProvinceSubWindow))
                    Engine.Open(Engine.CreateClass<SubWindow>(SubWindowType, new object[] { Tile.Owner }));
                else
                    Engine.Open(Engine.CreateClass<SubWindow>(SubWindowType, new object[] { Tile }));
            }
        }
        private void RightClick(object sender, MouseButtonEventArgs e)
        {
        }

        //- Set Coordinates
        public static int YCor = 100 + 1;
        public static int XCor = 90 + 1;
        public void SetCor(int x, int y)
        {
            X = x;
            Y = y;

            if (x % 2 == 1)
                Canvas.SetTop(this, 50 + Y * YCor);
            else
                Canvas.SetTop(this, Y * YCor);

            Canvas.SetLeft(this, X * XCor);
        }
        public double[] GetCanvasCors()
        {
            double left = Canvas.GetLeft(this);
            double top = Canvas.GetTop(this);
            left += 120 / 2;
            top += 100 / 2;
            return new double[] { left, top };
        }

        public List<HexagonTile> GetNeighbours()
        {
            List<HexagonTile> local = new List<HexagonTile>();
            HexagonTile[,] map = Engine.Map.Provinces;

            //- Always correct
            if (Y != (map.GetLength(1) - 1) && map[X, Y + 1] != null)
                local.Add(map[X, Y + 1]);
            if (Y != 0 && map[X, Y - 1] != null)
                local.Add(map[X, Y - 1]);

            if (X % 2 == 0)
            {
                if (Y != 0 && X != (map.GetLength(0) - 1) && map[X + 1, Y - 1] != null)
                    local.Add(map[X + 1, Y - 1]);
                if (X != (map.GetLength(0) - 1) && map[X + 1, Y] != null)
                    local.Add(map[X + 1, Y]);
                if (X != 0 && Y != 0 && map[X - 1, Y - 1] != null)
                    local.Add(map[X - 1, Y - 1]);
                if (X != 0 && map[X - 1, Y] != null)
                    local.Add(map[X - 1, Y]);
            }
            else
            {
                if (X != (map.GetLength(0) - 1) && map[X + 1, Y] != null)
                    local.Add(map[X + 1, Y]);
                if (Y != (map.GetLength(1) - 1) && X != (map.GetLength(0) - 1) && map[X + 1, Y + 1] != null)
                    local.Add(map[X + 1, Y + 1]);
                if (X != 0 && map[X - 1, Y] != null)
                    local.Add(map[X - 1, Y]);
                if (X != 0 && Y != (map.GetLength(1) - 1) && map[X - 1, Y + 1] != null)
                    local.Add(map[X - 1, Y + 1]);
            }
            return local;
        }
    }
}
