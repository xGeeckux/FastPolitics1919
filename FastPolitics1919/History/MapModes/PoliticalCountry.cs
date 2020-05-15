using FastPolitics1919.Common;
using FastPolitics1919.Data.Common.MapObjects;
using FastPolitics1919.Gfx;
using System.Windows.Media.Imaging;

namespace FastPolitics1919.History.MapModes
{
    public class PoliticalCountry : MapMode
    {
        public override BitmapImage Icon => Images.FromPath(Images.map_modes + "icon_political_country");

        public PoliticalCountry()
        {
            ID = 2;
            Name = "Politisch:Nationen";
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
            tile.Hex.ToggleCountryColor();
            tile.Hex.TogglePlayerIcon();
            tile.Hex.ToggleControllerColor();
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
