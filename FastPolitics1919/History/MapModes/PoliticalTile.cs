using FastPolitics1919.Common;
using FastPolitics1919.Gfx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FastPolitics1919.History.MapModes
{
    public class PoliticalTile : MapMode
    {
        public override BitmapImage Icon => Images.FromPath(Images.map_modes + "icon_political_city");

        public PoliticalTile()
        {
            ID = 0;
            Name = "Politisch:Tile";
            GuiBorder.ToolTip = Name;
        }

        //- OnSelection
        public override void OnSelection()
        {
            ForEveryTile();
        }
        public override void ForEvery(Tile tile)
        {
            tile.Hex.ToggleGraphics(0.8);
            tile.Hex.ToggleBlackColor();
            tile.Hex.ToggleOpenTile();
            tile.Hex.ToggleTileName();
            tile.Hex.TogglePlayerIcon();
            tile.Hex.ToggleUnit();

            tile.Hex.ToggleTest();
        }
        public override void OnHoverLeave(Tile tile)
        {
        }

        //- OnDeselection
        public override void OnDeselection()
        {
            base.OnDeselection();
        }

        //- Tooltip
        public override object GetTooltip(Tile tile) => base.GetDefaultTooltip(tile);
    }
}
