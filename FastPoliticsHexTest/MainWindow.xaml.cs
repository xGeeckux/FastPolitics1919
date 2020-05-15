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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FastPoliticsHexTest
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PolygonCreator creator;
        public MainWindow()
        {
            InitializeComponent();
            canvas.Children.Clear();
            creator = new PolygonCreator();
            creator.Canvas = canvas;
            creator.Init(MapScale);

            DragHandler.Window = this;
            DragHandler.MakeDragable(canvas, true);

            canvas.MouseWheel += Gui_grid_MouseWheel;
        }

        private double MapScale = 1;
        private void Gui_grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                MapScale += 0.25d;
                if (MapScale >= 5d)
                    MapScale = 5d;
            }

            else if (e.Delta < 0)
            {
                MapScale -= 0.25d;
                if (MapScale < 0.25d)
                    MapScale = 0.25d;
            }

            creator.Init(MapScale);
        }

    }
}
