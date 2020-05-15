using FastPolitics1919.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FastPolitics1919.Common
{
    public abstract class Title : GameObject
    {
        //- Value
        public int TitleValue { get; set; }
        public abstract BitmapImage Image { get; }
    }
}
