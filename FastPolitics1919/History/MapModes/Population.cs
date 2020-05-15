using FastPolitics1919.Common;
using FastPolitics1919.Data.Handlers;
using FastPolitics1919.Gfx;
using System.Windows.Media.Imaging;

namespace FastPolitics1919.History.MapModes
{
    public class Population : MapMode
    {
        public override BitmapImage Icon => Images.FromPath(Images.map_modes + "icon_population");

        public Population()
        {
            ID = 30;
            Name = "Bevölkerungsdichte";
            GuiBorder.ToolTip = Name;
        }

        //- OnSelection
        public override void OnSelection()
        {
            ForEveryTile();
        }
        public override void ForEvery(Tile tile)
        {
            tile.Hex.ToggleGraphics(0.2);
            tile.Hex.ToggleBlackColor();
            int pop = tile.CitizenAmount;
            int max = Citizen.MaxCitizens;
            if (pop > max)
                pop = max;
            double vale = ((double)byte.MaxValue / max) * pop;
            tile.Hex.Polygon.Fill = ColorHandler.ColorFromRGB(255 - (int)vale, 230, 255 - (int)vale);
            tile.Hex.ToggleOpenTile();
            tile.Hex.TogglePlayerIcon();
            tile.Hex.ToggleUnit();
        }

        //- OnDeselection
        public override void OnDeselection()
        {
            base.OnDeselection();
        }

        //- Tooltip
        public override object GetTooltip(Tile tile)
        {
            string tt = (string)GetDefaultTooltip(tile);
            return tt + "Pop: " + tile.CitizenAmount;
        }
    }
}
