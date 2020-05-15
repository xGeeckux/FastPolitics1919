using FastPolitics1919.Gfx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FastPolitics1919.History.Buildings
{
    public class ZirkusKrone : UniqueBuilding
    {
        public override int BuildTime => -1;

        public override BitmapImage Image => Images.FromPath(Images.unique_buildings + "icon_zirkuskrone");

        public override string Description => "Der Zirkus Krone ist eine große Attraktion und die Größte Versammlungsmöglichkeit in München.";
        
        public ZirkusKrone()
        {
            Name = "Zirkus Krone München";
        }
    }
}
