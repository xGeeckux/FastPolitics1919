using FastPolitics1919.Gfx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FastPolitics1919.History.Buildings
{
    public class Bank : UniqueBuilding
    {
        public override int BuildTime => 4 * 12 * 3;

        public override BitmapImage Image => Images.FromPath(Images.unique_buildings + "icon_bank");

        public override string Description => "Eine Bank, welche die Möglichkeit des Geld-Hortens anbietet.";

        public Common.Bank LocalBank { get; set; }
        public Bank(Common.Bank bank)
        {
            LocalBank = bank;
            Name = LocalBank.Name;
        }
    }
}
