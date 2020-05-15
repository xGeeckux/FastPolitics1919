using FastPolitics1919.Common;
using FastPolitics1919.Gfx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FastPolitics1919.History.Titles
{
    public class CityFounder : Title
    {
        public City City { get; set; }
        public override BitmapImage Image => Images.IconCity;

        public CityFounder(City city)
        {
            Name = "Gründer";
            City = city;
            TitleValue = 2;
        }
    }
}
