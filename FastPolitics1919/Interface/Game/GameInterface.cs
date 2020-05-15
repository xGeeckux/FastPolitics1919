using FastPolitics1919.Common;
using FastPolitics1919.Data.Common.MapObjects;
using FastPolitics1919.Data.Handlers;
using FastPolitics1919.Gfx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace FastPolitics1919.Interface.Game
{
    public class GameInterface : BaseInterface
    {
        public override string Sign => "GameInterface";

        public new GameWindow Window => (GameWindow)base.Window;

        public override ImageSource WindowIcon => Gfx.Images.IconQuestionmark;
        public override string WindowTitle => "Game-Window";

        public override void Activate() => base.Window = new GameWindow();

        public GameInterface()
        {
            Show();
        }

        public override void Init()
        {
            base.Init();
            LoadGUI();

            //- debug
            Window.btn_debug.Content = "Current Round: " + Engine.Game.Current.Number;
            Window.btn_debug.Click += DebugBlick;
        }

        private void DebugBlick(object sender, System.Windows.RoutedEventArgs e)
        {
            Engine.Game.Update();
            Window.btn_debug.Content = "Current Round: " + Engine.Game.Current.Number;
            Log.Write("Next Round !");
        }

        private void LoadGUI()
        {
            LoadGUIMapModes();
            Update();
            #region Units
            LoadUnit();
            #endregion
        }
        public void Update()
        {
            LoadTop();
            if (Engine.CurrentPerson != null)
                LoadBottom();
            else
                Window.gui_grid.Children.Remove(Window.grid_bottom_gui);
        }

        #region Top
        private void LoadTop()
        {
            Window.img_money.Source = Images.IconMoney;
            Window.img_time.Source = Images.IconTime;

            Window.lbl_money.Content = "???";
            Window.lbl_time.Content = "???";

            if (Engine.CurrentPerson != null)
            {
                Person person = Engine.CurrentPerson;
                Window.lbl_money.Content = person.Money;
                if (person is Player player)
                {
                    Window.lbl_time.Content = player.CurActionPoints + " / " + player.MaxActionPoints;
                }
            }
        }
        #endregion

        #region Bottom
        private void LoadBottom()
        {
            Person person = Engine.CurrentPerson;

            if (!Window.gui_grid.Children.Contains(Window.grid_bottom_gui))
                Window.gui_grid.Children.Add(Window.grid_bottom_gui);
            Window.img_bottom_person.Source = Images.IconPerson;
            if (person.Image != null)
                Window.img_bottom_person.Source = person.Image;
            Window.btn_bottom_person.Click += ClickPerson;
            Window.lbl_bottom_person_name.Content = person.Name;
        }

        private void ClickPerson(object sender, System.Windows.RoutedEventArgs e)
        {
            Engine.OpenPerson(Engine.CurrentPerson);
        }
        #endregion

        #region Gui-Mapmodes
        private void LoadGUIMapModes()
        {
            ((Grid)Window.gui_border_right_bottom_exp.Parent).Children.Remove(Window.gui_border_right_bottom_exp);
            Window.gui_border_right_bottom_imp.MouseEnter += gui_border_right_bottom_hover;
            Window.gui_border_right_bottom_exp.MouseLeave += gui_border_right_bottom_impendet;

            Window.gui_panel_mapmodes.Children.Clear();
            int count = 0;
            StackPanel hor_panel = new StackPanel();
            MapMode[] modes = Engine.Game.EveryMapMode.Get();
            for (int i = 0; i < modes.Length; i++)
            {
                if (count == 0)
                {
                    hor_panel = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        VerticalAlignment = System.Windows.VerticalAlignment.Top,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                        Margin = new System.Windows.Thickness(0, 0, 0, 4)
                    };
                }
                count++;

                hor_panel.Children.Add(modes[i].GuiBorder);
                if (count == 4)
                {
                    count = 0;
                    Window.gui_panel_mapmodes.Children.Add(hor_panel);
                }
            }
            if (!Window.gui_panel_mapmodes.Children.Contains(hor_panel))
                Window.gui_panel_mapmodes.Children.Add(hor_panel);
        }
        //- Hover and Leave
        private void gui_border_right_bottom_hover(object sender, MouseEventArgs e)
        {
            ((Grid)Window.gui_border_right_bottom_imp.Parent).Children.Add(Window.gui_border_right_bottom_exp);
        }
        private void gui_border_right_bottom_impendet(object sender, MouseEventArgs e)
        {
            ((Grid)Window.gui_border_right_bottom_exp.Parent).Children.Remove(Window.gui_border_right_bottom_exp);
        }
        #endregion

        #region Unit
        private void LoadUnit()
        {
            CloseUnit();
            Window.btn_unit_exit.Click += UnitButtonExit;
            Window.btn_unit_link.Click += LinkClick;
            Window.btn_unit_order_cancel.Click += OrderCanel;

            Window.lbl_unit_location.MouseLeftButtonUp += LocationClick;
            HoverHandler.AddHover(Window.lbl_unit_location);

            Window.lbl_unit_commander.MouseLeftButtonUp += CommanderClick;
            HoverHandler.AddHover(Window.lbl_unit_commander);

            Window.img_unit_commander.MouseLeftButtonUp += CommanderClick;
        }
        private void CommanderClick(object sender, MouseButtonEventArgs e)
        {
            Person person = (Person)((FrameworkElement)sender).Tag;
            Engine.OpenPerson(person);
        }
        private void LocationClick(object sender, MouseButtonEventArgs e)
        {
            Tile tile = (Tile)((FrameworkElement)sender).Tag;
            Engine.OpenTile(tile);
        }

        private void UnitButtonExit(object sender, System.Windows.RoutedEventArgs e)
        {
            Engine.Map.SetCurrentSelected(Engine.Map.CurrentSelected);
        }
        public void OpenUnit(Unit unit)
        {
            CloseUnit();
            Window.gui_grid.Children.Add(Window.gui_grid_unit_left);

            #region Parents
            Window.panel_unit_parents.Children.Clear();
            Unit parent = unit.Parent;
            List<UIElement> parents = new List<UIElement>();
            int c = 0;
            while (true)
            {
                if (parent == null)
                    break;
                Window.btn_unit_parent.Content = parent.Name;
                Button btn = (Button)SubWindow.CopyFrom(Window.btn_unit_parent);
                btn.Click += ParentClick;
                btn.Tag = parent;
                parents.Add(btn);
                parent = parent.Parent;
                c++;
            }
            parents.Reverse();
            foreach (UIElement element in parents)
                Window.panel_unit_parents.Children.Add(element);
            #endregion

            #region Top
            Window.img_unit_commander.Source = Images.IconPerson;
            Window.lbl_unit_commander.Content = "Kein Befehlshaber";
            Window.img_unit_main_size.Source = unit.Symbol;
            Window.img_unit_main_counter.Source = unit.IconCounter;
            Window.lbl_unit_name.Content = unit.Name;
            Window.lbl_unit_amount.Content = unit.CurStrength;

            Window.proBar_unit_org.Maximum = unit.MaxOrganisation;
            Window.proBar_unit_org.Value = unit.CurOrganisation;
            Window.proBar_unit_org.ToolTip = unit.CurOrganisation + " / " + unit.MaxOrganisation;

            Window.proBar_unit_strenght.Maximum = unit.MaxStrength;
            Window.proBar_unit_strenght.Value = unit.CurStrength;
            Window.proBar_unit_strenght.ToolTip = unit.CurStrength + " / " + unit.MaxStrength;

            if (unit.Commander != null)
            {
                Window.img_unit_commander.Source = unit.Commander.Image;
                Window.img_unit_commander.Tag = unit.Commander;
                Window.lbl_unit_commander.Content = unit.Commander.Name;
                Window.lbl_unit_commander.Tag = unit.Commander;
            }
            #endregion

            #region Center
            Window.lbl_unit_location.Content = unit.Location.Name;
            Window.lbl_unit_location.Tag = unit.Location;
            if (unit.IsLinked)
                Window.btn_unit_link.Content = "Link-Out";
            else
                Window.btn_unit_link.Content = "Linkup";
            Window.btn_unit_link.Tag = unit;
            Window.btn_unit_link.IsEnabled = true;
            if (unit.Parent == null || unit.Parent.LocationID != unit.LocationID)
                Window.btn_unit_link.IsEnabled = false;

            //- Orders
            Window.lbl_unit_order.Content = "Kein Befehl";
            if (unit.Orders.Count != 0)
            {
                Order order = unit.Orders[0];
                Window.lbl_unit_order.Content = order.OrderText;
                Window.btn_unit_order_cancel.Tag = unit;
            }
            #endregion

            #region LocalUnits
            Window.panel_unit_localunits.Children.Clear();
            Window.lbl_unit_localunit_count.Content = unit.LocalUnits.Count;
            foreach (Unit local_unit in unit.LocalUnits)
            {
                Window.img_localunit_counter.Source = local_unit.IconCounter;
                Window.img_localunit_size.Source = local_unit.Symbol;
                Window.lbl_localunit_name.Content = local_unit.Name;
                Window.lbl_localunit_size.Content = local_unit.CurStrength;
                Window.proBar_localunit_org.Maximum = local_unit.MaxOrganisation;
                Window.proBar_localunit_org.Value = local_unit.CurOrganisation;
                Window.proBar_localunit_strenght.Maximum = local_unit.MaxStrength;
                Window.proBar_localunit_strenght.Value = local_unit.CurStrength;

                Border sub = (Border)SubWindow.CopyFrom(Window.border_localunit_vorlage);
                HoverHandler.AddHover(sub);
                Window.panel_unit_localunits.Children.Add(sub);
            }
            #endregion

            #region SubUnits
            Window.panel_unit_subunits.Children.Clear();
            Window.lbl_unit_subunit_count.Content = unit.SubUnits.Count;
            int subunits_amount = 0;
            foreach (Unit sub_unit in unit.SubUnits)
            {
                //- Mark other Units in Unit
                SubUnitBorders.Add(sub_unit);
                if (sub_unit.BorderParent != null)
                    sub_unit.BorderParent.Background = ColorHandler.ColorFromRGB("10-10-220");

                Window.img_subunit_counter.Source = sub_unit.IconCounter;
                Window.img_subunit_size.Source = sub_unit.Symbol;
                Window.lbl_subunit_name.Content = sub_unit.Name;
                Window.lbl_subunit_size.Content = sub_unit.CurStrength;
                Window.proBar_subunit_org.Maximum = sub_unit.MaxOrganisation;
                Window.proBar_subunit_org.Value = sub_unit.CurOrganisation;
                Window.proBar_subunit_strenght.Maximum = sub_unit.MaxStrength;
                Window.proBar_subunit_strenght.Value = sub_unit.CurStrength;
                Window.lbl_subunit_commander.Content = "Kein Befehlshaber";
                if (sub_unit.Commander != null)
                    Window.lbl_subunit_commander.Content = sub_unit.Commander.Name;

                subunits_amount += sub_unit.CurStrength;

                Border sub = (Border)SubWindow.CopyFrom(Window.border_unit_vorlage);
                sub.Tag = sub_unit;
                sub.MouseLeftButtonUp += SubUnitClick;
                HoverHandler.AddHover(sub);
                Window.panel_unit_subunits.Children.Add(sub);
            }
            Window.lbl_unit_subunit_amount.Content = subunits_amount;
            #endregion

            //- Parent
            if (unit.Parent != null && unit.Parent.BorderParent != null)
            {
                unit.Parent.BorderParent.Background = ColorHandler.ColorFromRGB("20-220-20");
                SubUnitParten = unit.Parent;
            }
        }

        private void SubUnitClick(object sender, MouseButtonEventArgs e)
        {
            Unit unit = (Unit)((Border)sender).Tag;
            Engine.Map.SetCurrentSelected(unit);
        }
        private void ParentClick(object sender, RoutedEventArgs e)
        {
            Unit unit = (Unit)((Button)sender).Tag;
            Engine.Map.SetCurrentSelected(unit);
        }
        private void OrderCanel(object sender, RoutedEventArgs e)
        {
            Unit unit = (Unit)((Button)sender).Tag;
            if (unit.Orders.Count != 0)
                unit.Orders.Remove(unit.Orders[0]);
            OpenUnit(unit);
        }

        private void LinkClick(object sender, System.Windows.RoutedEventArgs e)
        {
            Unit unit = (Unit)((Button)sender).Tag;
            if (unit.Parent != null)
            {
                if (unit.IsLinked)
                    unit.Parent.LinkOut(unit);
                else
                    unit.Parent.LinkUp(unit);
                OpenUnit(unit);
            }
        }

        private List<Unit> SubUnitBorders = new List<Unit>();
        private Unit SubUnitParten = null;
        public void CloseUnit()
        {
            if (Window.gui_grid.Children.Contains(Window.gui_grid_unit_left))
                Window.gui_grid.Children.Remove(Window.gui_grid_unit_left);
            foreach (Unit unit in SubUnitBorders)
                if (unit.BorderParent != null)
                    unit.BorderParent.Background = ColorHandler.ColorFromRGB(unit.Color);
            SubUnitBorders.Clear();
            if (SubUnitParten != null)
                SubUnitParten.BorderParent.Background = ColorHandler.ColorFromRGB(SubUnitParten.Color);
        }
        #endregion

        public override void OnPress(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Back:
                    DragHandler.ResetPositions(Engine.Map, 0, 0);
                    break;
                default:
                    break;
            }
        }
    }
}
