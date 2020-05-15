using FastPolitics1919.Common;
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
    public class NeighbourMode : MapMode
    {
        public override BitmapImage Icon => Images.IconQuestionmark;

        public NeighbourMode()
        {
            ID = 99;
            Name = "Hover";
            GuiBorder.ToolTip = Name;
        }

        //- OnSelection
        public override void OnSelection()
        {
            ForEveryTile();
        }
        public override void ForEvery(Tile tile)
        {
            tile.Hex.ToggleBlackColor();
        }
        public override void OnHover(Tile tile)
        {
            tile.Hex.Polygon.Fill = Brushes.Gray;
            List<Tile> list = tile.GetNeighbours();
            foreach (Tile t in list)
            {
                t.Hex.Polygon.Fill = Brushes.LightBlue;
            }
        }
        public override void OnHoverLeave(Tile tile)
        {
            List<Tile> list = tile.GetNeighbours();
            foreach (Tile t in list)
            {
                t.Hex.ToggleBlackColor();
            }
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
