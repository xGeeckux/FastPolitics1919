using FastPolitics1919.Gfx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FastPolitics1919.History.Buildings
{
    public class SimpleGovernmentBuilding : GovernmentBuilding
    {
        public override int BuildTime => 32;

        public override BitmapImage Image => Images.IconGovernmentBuilding;

        public override string Description => "Ein Regierungsgebäude wo das Parlament tagt.";

        public SimpleGovernmentBuilding()
        {
            Name = "Regierungsgebäude";
        }
    }
}
