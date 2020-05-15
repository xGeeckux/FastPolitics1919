using FastPolitics1919.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FastPolitics1919.Common
{
    public abstract class Building : GameObject
    {
        public static int Index = 0;
        //- Process
        public override bool IsProcessable => true;
        public override int ProcessRound => BuildTime;

        //- Building Time
        public abstract int BuildTime { get; }

        //- Image
        public abstract BitmapImage Image { get; }

        //- Ancestor
        public Type Ancestor { get; set; }

        //- Location
        public int LocationID { get; set; }
        public City Location => Engine.Game.FindCity(LocationID);

        //- Info
        public abstract string Description { get; }

        //- Constructor
        public Building()
        {
            Index++;
            ID = Index;
            Engine.Game.Buildings.Add(this, ID);
        }
    }
}
