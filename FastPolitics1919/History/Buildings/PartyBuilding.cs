using FastPolitics1919.Common;
using FastPolitics1919.Gfx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FastPolitics1919.History.Buildings
{
    public class PartyBuilding : UniqueBuilding
    {
        //- Party
        public Party Party { get; set; }

        public override int BuildTime => 24;

        public override BitmapImage Image => Images.IconPartyBuilding;

        public override string Description => "Die Parteizentrale der: " + Party.Name + " (" + Party.ShortName + ")";
    }
}
