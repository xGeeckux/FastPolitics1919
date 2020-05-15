using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FastPoliticsHexTest
{
    public class PolygonCreator
    {

        public Canvas Canvas { get; set; }

        public int XLength { get; set; } = 20;
        public int YLength => (int)(XLength * 1.75);

        public Grid[,] Map { get; set; }
        private List<Polygon> OriginalPolygons;
        public void Init(double scale)
        {
            Map = new Grid[XLength, YLength];
            Canvas.Children.Clear();

            for (int y = 0; y < YLength; y++)
            {
                for (int x = 0; x < XLength; x++)
                {
                    Map[x, y] = new Grid();
                    Polygon local_polygon = new Polygon
                    {
                        Fill = Brushes.Black,
                        Points = new PointCollection
                        {
                            new Point(30, 0),
                            new Point(90, 0),
                            new Point(120, 50),
                            new Point(90, 100),
                            new Point(30, 100),
                            new Point(0, 50)
                        }
                    };
                    local_polygon.Tag = x + ":" + y;
                    Map[x, y].Children.Add(local_polygon);
                    OriginalPolygons.Add(local_polygon);
                    Label lbl = new Label();
                    lbl.Content = "(" + x + "," + y + ")";
                    lbl.Foreground = Brushes.White;
                    lbl.VerticalAlignment = VerticalAlignment.Center;
                    lbl.HorizontalAlignment = HorizontalAlignment.Center;
                    Map[x, y].Children.Add(lbl);

                    if (x % 2 == 1)
                        Canvas.SetTop(Map[x, y], 50 + y * 101);
                    else
                        Canvas.SetTop(Map[x, y], y * 101);

                    Canvas.SetLeft(Map[x, y], x * 91);
                    Canvas.Children.Add(Map[x, y]);
                }
            }
            //<Polygon Points="30,0 90,0 120,50 90,100 30,100 0,50" Fill="Black"/>
        }
    }
}
