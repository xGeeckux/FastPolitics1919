using FastPolitics1919.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FastPolitics1919.Interface.Game
{
    public class CitySubWindow : SubWindow
    {
        //- Variable
        public Tile Tile { get; set; }
        public CityWindow Window { get; set; }

        //- Constructor
        public CitySubWindow(Tile tile)
        {
            Tile = tile;
            Constructor(Window = new CityWindow(Tile));
        }

        //- Load City specific content
        protected override void InternLoad()
        {

        }

        public override void Update() => Window.Update();

        //- City Window Exit
        protected override void ExitWindow()
        {
            //- Deselect
            if (Engine.Map.CurrentSelected is Tile && Engine.Map.CurrentSelected == Tile)
                Engine.Map.SetCurrentSelected(Engine.Map.CurrentSelected);
        }
    }
}
