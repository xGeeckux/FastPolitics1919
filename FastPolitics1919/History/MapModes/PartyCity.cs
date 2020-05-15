using FastPolitics1919.Common;
using FastPolitics1919.Data.Common.MapObjects;
using FastPolitics1919.Data.Handlers;
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
    public class PartyCity : MapMode
    {
        public override BitmapImage Icon => Images.FromPath(Images.map_modes + "icon_partycity");

        public PartyCity()
        {
            ID = 20;
            Name = "Partei:Stadt";
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
            
            if (tile is Landscape)
                hex.Polygon.Fill = Brushes.Black;
            if (tile is City city)
            {
                if (city.Government != null)
                    hex.Polygon.Fill = ColorHandler.ColorFromRGB(city.Government.RulingParties[0].Color);
                else
                    hex.Polygon.Fill = Brushes.DarkSlateGray;
            }
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
            if (tile is City city && city.Government != null)
            {
                return tt + "Regierungspartei : " + city.Government.RulingParties[0].Name;
            }
            return tt;
        }
    }
}
