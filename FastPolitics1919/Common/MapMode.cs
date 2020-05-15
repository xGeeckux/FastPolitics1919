using FastPolitics1919.Data.Common;
using FastPolitics1919.Data.Handlers;
using FastPolitics1919.Gfx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace FastPolitics1919.Common
{
    public abstract class MapMode : GameObject
    {
        //- For Ids
        public static int Index = 0;

        //- GUI
        public Border GuiBorder { get; set; }
        public virtual BitmapImage Icon => Images.IconQuestionmark;
        
        //- Constructor
        public MapMode()
        {
            GuiBorder = new Border
            {
                BorderBrush = ColorHandler.ColorFromHex("FFCBA112"),
                BorderThickness = new System.Windows.Thickness(2),
                CornerRadius = new System.Windows.CornerRadius(25),
                Width = 30,
                Height = 30,
                Background = ColorHandler.ColorFromHex("FF4C518B"),
                Margin = new System.Windows.Thickness(0, 0, 4, 0)
            };

            Button btn = new Button
            {
                Background = null,
                BorderBrush = null,
                Foreground = null
            };
            btn.Click += Click;
            GuiBorder.Child = btn;

            Image img = new Image
            {
                Source = Icon,
                Margin = new System.Windows.Thickness(-3)
            };
            btn.Content = img;
        }

        //- Eventhandler Selection
        public abstract void OnSelection();
        //- Eventhandler Deseleciton
        public virtual void OnDeselection()
        {
            foreach (Tile tile in Engine.Game.Tiles.Get())
                tile.Hex.ResetEveryThing();
        }
        //- Tooltip For Hex
        public abstract object GetTooltip(Tile tile);
        //- For Hover override
        public virtual void OnHover(Tile tile)
        {

        }
        public virtual void OnHoverLeave(Tile tile)
        {

        }

        //- Default Tooltip
        protected object GetDefaultTooltip(Tile tile)
        {
            string txt = "";
            txt += "[" + tile.ID + "]" + " " + tile.Name + "\n";
            return txt;
        }

        //- Help Methode
        public void ForEveryTile()
        {
            foreach (Tile tile in Engine.Game.Tiles.Get())
            {
                ForEvery(tile);
                tile.Hex.Polygon.ToolTip = GetTooltip(tile);
            }
        }
        public virtual void ForEvery(Tile tile) { }

        //- Real Eventhandler
        private void Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Engine.Map.ChangeMapMode(this);
        }
    }
}
