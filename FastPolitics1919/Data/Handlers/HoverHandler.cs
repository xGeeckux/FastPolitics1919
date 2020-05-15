using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FastPolitics1919.Data.Handlers
{
    public class HoverHandler
    {
        public static void AddHover(Control element)
        {
            HoverHandler handler = new HoverHandler(element, element.Background);
        }
        public static void AddHover(Border element)
        {
            HoverHandler handler = new HoverHandler(element, element.Background);
        }
        private static void ChangeBackground(UIElement element, Brush brush)
        {
            if (element is Control control)
                control.Background = brush;
            if (element is Border border)
                border.Background = brush;
        }

        public UIElement Element { get; set; }
        private Brush DefaultBrush { get; set; }
        private Brush HoverBrush { get; set; }
        private HoverHandler(UIElement element, Brush db)
        {
            Element = element;
            element.MouseEnter += Hover;
            element.MouseLeave += Leave;
            
            DefaultBrush = db;

            LoadHoverBrush();
        }

        private void LoadHoverBrush()
        {

            if (!(DefaultBrush is SolidColorBrush))
            {
                HoverBrush = new SolidColorBrush(Color.FromArgb(150, 200, 200, 200));
                return;
            }

            Color color = ((SolidColorBrush)DefaultBrush).Color;
            int[] rgb = { color.R, color.G, color.B };
            for (int i = 0; i < rgb.Length; i++)
            {
                rgb[i] = (int)(rgb[i] * 1.25);
                if (rgb[i] > 255)
                    rgb[i] = 255;
            }
            HoverBrush = new SolidColorBrush(Color.FromRgb((byte)rgb[0], (byte)rgb[1], (byte)rgb[2]));
        }

        private void Hover(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ChangeBackground(Element, HoverBrush);
        }
        private void Leave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ChangeBackground(Element, DefaultBrush);
        }
    }
}
