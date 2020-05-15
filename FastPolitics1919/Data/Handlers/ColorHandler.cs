using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FastPolitics1919.Data.Handlers
{
    public static class ColorHandler
    {
        public static SolidColorBrush ColorFromHex(string hex)
        {
            if (hex.Substring(0, 1) == "#")
                return (SolidColorBrush)new BrushConverter().ConvertFrom(hex);
            return (SolidColorBrush)new BrushConverter().ConvertFrom("#" + hex);
        }

        public static SolidColorBrush ColorFromText(string color)
        {
            return ColorFromRGB(color);
        }

        public static SolidColorBrush ColorFromRGB(string color, byte alpha)
        {
            if (color == null || color.Length == 0)
                return ColorFromARGB(255, 0, 0, 0);
            string[] splitted = color.Split('-');
            if (splitted.Length != 3)
                return ColorFromARGB(255, 0, 0, 0);
            byte[] bytes = new byte[3] { Convert.ToByte(splitted[0]), Convert.ToByte(splitted[1]), Convert.ToByte(splitted[2]) };

            return ColorFromARGB(alpha, bytes[0], bytes[1], bytes[2]);
        }
        public static SolidColorBrush ColorFromRGB(string color)
        {
            return ColorFromRGB(color, 255);
        }
        public static SolidColorBrush ColorFromRGB(int r, int g, int b)
        {
            return ColorFromRGB((byte)r, (byte)g, (byte)b);
        }
        public static SolidColorBrush ColorFromRGB(byte r, byte g, byte b)
        {
            return ColorFromARGB(255, r, g, b);
        }
        public static SolidColorBrush ColorFromARGB(byte a, byte r, byte g, byte b)
        {
            Color color = Color.FromArgb(a, r, g, b);
            return new SolidColorBrush(color);
        }
    }
}
