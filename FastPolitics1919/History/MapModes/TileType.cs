using FastPolitics1919.Common;
using FastPolitics1919.Data.Common.MapObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FastPolitics1919.History.MapModes
{
    public class TileType : MapMode
    {
        public TileType()
        {
            ID = 10;
            Name = "Stadt Arten";
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
            tile.Hex.ToggleOpenTile();
            tile.Hex.ToggleTileName();
            tile.Hex.TogglePlayerIcon();

            HexagonTile hex = tile.Hex;

            if (tile is Tile)
                hex.Polygon.Fill = Brushes.Black;
            if (tile is Landscape)
                hex.Polygon.Fill = Brushes.DarkCyan;
            if (tile is City)
                hex.Polygon.Fill = Brushes.DarkGray;
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
            return tt + tile.GetType().Name;
        }
    }
}
