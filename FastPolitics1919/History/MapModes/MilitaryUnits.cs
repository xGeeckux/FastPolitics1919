using FastPolitics1919.Common;
using FastPolitics1919.Data.Handlers;
using FastPolitics1919.Gfx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FastPolitics1919.History.MapModes
{
    public class MilitaryUnits : MapMode
    {
        public override BitmapImage Icon => Images.FromPath(Images.map_modes + "icon_military_units");

        public MilitaryUnits()
        {
            ID = 20;
            Name = "Landeinheiten";
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
            if (tile.Units.Count > 0)
            {
                tile.Hex.Polygon.Fill = ColorHandler.ColorFromRGB("50-200-50");
                foreach (Unit unit in tile.Units)
                {
                    if (unit.IsHostile)
                    {
                        tile.Hex.Polygon.Fill = ColorHandler.ColorFromRGB("200-0-0");
                        break;
                    }
                }
                if (tile.Battles.Count > 0) tile.Hex.Polygon.Fill = ColorHandler.ColorFromRGB("200-200-0");
            }
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
