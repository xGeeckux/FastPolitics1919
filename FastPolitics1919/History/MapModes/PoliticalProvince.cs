using FastPolitics1919.Common;
using FastPolitics1919.Gfx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FastPolitics1919.History.MapModes
{
    public class PoliticalProvince : MapMode
    {
        public override BitmapImage Icon => Images.FromPath(Images.map_modes + "icon_political_province");

        public PoliticalProvince()
        {
            ID = 1;
            Name = "Politisch:Provinzen";
            GuiBorder.ToolTip = Name;
        }

        //- OnSelection
        public override void OnSelection()
        {
            ForEveryTile();
        }
        public override void ForEvery(Tile tile)
        {
            tile.Hex.ToggleGraphics(0.6);
            tile.Hex.ToggleTileName();
            tile.Hex.ToggleOpenProvince();
            tile.Hex.ToggleProvinceColor();
            tile.Hex.TogglePlayerIcon();
            tile.Hex.ToggleUnit();
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
