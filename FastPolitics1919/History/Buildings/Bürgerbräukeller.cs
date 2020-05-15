using FastPolitics1919.Gfx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FastPolitics1919.History.Buildings
{
    public class Bürgerbräukeller : UniqueBuilding
    {
        public override int BuildTime => -1;

        public override BitmapImage Image => Images.FromPath(Images.unique_buildings + "icon_bürgerbräukeller");

        public override string Description => "Der Bürgerbräukeller ist einer der in den 20er Jahren berühmtesten Orten für kleine Parteien.";

        public Bürgerbräukeller()
        {
            Name = "Bürgerbräukeller";
        }
    }
}
