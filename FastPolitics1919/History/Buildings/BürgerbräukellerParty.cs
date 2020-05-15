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
    public class BürgerbräukellerParty : PartyBuilding
    {
        public override int BuildTime => 12;

        public override BitmapImage Image => Images.FromPath(Images.unique_buildings + "icon_bürgerbräukeller");

        public BürgerbräukellerParty(Party party)
        {
            Name = "Bürgerbräukeller Parteizentrale";
            Party = party;
            Ancestor = typeof(Bürgerbräukeller);
        }
    }
}
