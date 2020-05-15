using FastPolitics1919.Data.Handlers;
using FastPolitics1919.Gfx;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;

namespace FastPolitics1919.Interface
{
    public abstract class SubWindow : Grid
    {
        //- Variable
        protected BitmapImage Icon { get; set; }
        protected Grid FullGrid { get; set; }
        protected Image WindowIcon { get; set; }

        //- Constructor
        protected void Constructor(Window window)
        {
            LoadGrid();

            //- Load Grid
            if (window == null)
                return;
            if (!(window.Content is Grid))
                return;
            Grid grid = (Grid)window.Content;

            //- Use Window
            WindowName.Content = window.Title;
            VerticalAlignment = grid.VerticalAlignment;
            HorizontalAlignment = grid.HorizontalAlignment;
            Margin = new Thickness(grid.Margin.Left, grid.Margin.Top, grid.Margin.Right, grid.Margin.Bottom);
            grid.Margin = new Thickness(0, 60, 0, 0);

            //- Kill Window
            window.Content = null;
            if (window.Icon != null)
                SetIcon((BitmapImage)window.Icon);
            window = null;

            FullGrid.Children.Add(grid);
            InternLoad();
        }

        //- Secound Constructor
        protected Border Border { get; set; }
        protected Grid TopGrid { get; set; }
        protected Label WindowName { get; set; }
        protected virtual void LoadGrid()
        {
            Border = new Border
            {
                Background = Brushes.DarkGray,
                BorderThickness = new Thickness(3),
                BorderBrush = Brushes.Black,
                CornerRadius = new CornerRadius(6, 6, 3, 3)
            };
            Children.Add(Border);

            FullGrid = new Grid();
            Border.Child = FullGrid;

            TopGrid = new Grid
            {
                Background = ColorHandler.ColorFromHex("#7F000000"),
                VerticalAlignment = VerticalAlignment.Top,
                Height = 35,
                Margin = new Thickness(0, 15, 0, 0)
            };
            FullGrid.Children.Add(TopGrid);

            WindowIcon = new Image
            {
                Margin = new Thickness(10, 0, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left
            };
            TopGrid.Children.Add(WindowIcon);

            WindowName = new Label
            {
                Padding = new Thickness(0),
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                FontWeight = FontWeights.Bold,
                FontSize = 18,
                Margin = new Thickness(0),
                Foreground = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                FontFamily = new FontFamily("Arial")
            };
            TopGrid.Children.Add(WindowName);

            Button btn_exit = new Button
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                Background = null,
                BorderBrush = null,
                Padding = new Thickness(0),
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                VerticalContentAlignment = VerticalAlignment.Stretch,
                Width = 35,
                Height = 35,
                Margin = new Thickness(0, 0, 10, 0)
            };
            btn_exit.Click += Exit;
            TopGrid.Children.Add(btn_exit);

            Border btn_border = new Border
            {
                BorderBrush = Brushes.Black,
                Background = ColorHandler.ColorFromHex("#FF666666"),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(15)
            };
            btn_exit.Content = btn_border;

            Label lbl_border = new Label
            {
                Content = "X",
                Padding = new Thickness(0),
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                FontWeight = FontWeights.Bold,
                FontSize = 18
            };
            btn_border.Child = lbl_border;

            //- Additional Load
            SetIcon(Images.IconQuestionmark);
            DragHandler.MakeDragable(this);
        }
        public void Close()
        {
            Engine.Close(this);
            ExitWindow();
        }
        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }
        protected virtual void ExitWindow()
        {

        }

        //- Third Constructor
        protected abstract void InternLoad();

        //- Update if neccessary
        public abstract void Update();

        //- GUI Icon
        protected virtual void SetIcon(BitmapImage icon)
        {
            Icon = icon;
            WindowIcon.Source = Icon;
        }

        //- Copy GUI Objects from Xaml
        public static UIElement CopyFrom(object _obj)
        {
            if (_obj == null)
                return null;
            string saved = XamlWriter.Save(_obj);
            StringReader sReader = new StringReader(saved);
            XmlReader xReader = XmlReader.Create(sReader);
            UIElement element = (UIElement)XamlReader.Load(xReader);
            return element;
        }
    }
}
