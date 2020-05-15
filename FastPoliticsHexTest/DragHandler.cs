using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FastPoliticsHexTest
{
    public class DragHandler
    {
        public static MainWindow Window { get; set; }
        public static void MakeDragable(UIElement element, bool middle_mouse)
        {
            TranslateTransform tt = new TranslateTransform();
            element.RenderTransform = tt;

            if (middle_mouse)
            {
                element.MouseMove += mouse_move_middle;
                element.MouseDown += mouse_down_middle;
                element.MouseUp += mouse_up_middle;
            }
            else
            {
                element.MouseMove += mouse_move;
                element.MouseLeftButtonDown += mouse_down;
                element.MouseLeftButtonUp += mouse_up;
            }
        }
        public static void MakeDragable(UIElement element)
        {
            MakeDragable(element, false);
        }
        public static void ResetPositions(UIElement element)
        {
            ResetPositions(element, 0, 0);
        }
        public static void ResetPositions(UIElement element, int x, int y)
        {
            ((TranslateTransform)element.RenderTransform).X = x;
            ((TranslateTransform)element.RenderTransform).Y = y;
        }

        private static Point m_start;
        private static Vector m_startOffset;

        //- Modified with only the middle mouse click
        private static void mouse_up_middle(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            switch (e.ChangedButton)
            {
                case System.Windows.Input.MouseButton.Middle:
                    UIElement grid = (UIElement)sender;
                    grid.ReleaseMouseCapture();
                    break;
            }
        }
        private static void mouse_move_middle(object sender, System.Windows.Input.MouseEventArgs e)
        {
            UIElement grid = (UIElement)sender;
            if (grid.IsMouseCaptured)
            {
                Vector offset = Point.Subtract(e.GetPosition(Window), m_start);

                ((TranslateTransform)grid.RenderTransform).X = m_startOffset.X + offset.X;
                ((TranslateTransform)grid.RenderTransform).Y = m_startOffset.Y + offset.Y;
            }
        }
        private static void mouse_down_middle(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            switch (e.ChangedButton)
            {
                case System.Windows.Input.MouseButton.Middle:
                    UIElement grid = (UIElement)sender;
                    m_start = e.GetPosition(Window);
                    m_startOffset = new Vector(((TranslateTransform)grid.RenderTransform).X, ((TranslateTransform)grid.RenderTransform).Y);
                    grid.CaptureMouse();
                    break;
            }
        }


        //- Default, with all mouse-keys
        private static void mouse_up(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            UIElement grid = (UIElement)sender;
            grid.ReleaseMouseCapture();
        }
        private static void mouse_move(object sender, System.Windows.Input.MouseEventArgs e)
        {
            UIElement grid = (UIElement)sender;
            if (grid.IsMouseCaptured)
            {
                Vector offset = Point.Subtract(e.GetPosition(Window), m_start);

                ((TranslateTransform)grid.RenderTransform).X = m_startOffset.X + offset.X;
                ((TranslateTransform)grid.RenderTransform).Y = m_startOffset.Y + offset.Y;
            }
        }
        private static void mouse_down(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            UIElement grid = (UIElement)sender;
            m_start = e.GetPosition(Window);
            m_startOffset = new Vector(((TranslateTransform)grid.RenderTransform).X, ((TranslateTransform)grid.RenderTransform).Y);
            grid.CaptureMouse();
        }
    }
}
