using FastPolitics1919.Common;
using FastPolitics1919.Data.Common.MapObjects;
using FastPolitics1919.Gfx;
using FastPolitics1919.History.Titles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPolitics1919.Data.Common.BuildProcesses
{
    public class FoundCityProcess : BuildProcess
    {
        public Tile AdditionalObject { get; set; }

        public FoundCityProcess(Tile tile)
        {
            AdditionalObject = tile;
            AdditionalObject.Upgrading = true;
            Load(tile);
        }

        public override void OnBuild()
        {
            ((Tile)GameObject).Founder = Engine.CurrentPerson;
        }

        public override void Done()
        {
            base.Done();
            Tile tile = Tile.FromTile(AdditionalObject);
            Engine.Game.Tiles.Remove(tile.ID);
            Engine.Game.Tiles.Add(tile, tile.ID);
            tile.BackGroundImage = Images.TileCity;
            Log.Write("Province upgraded !" + tile.ID);
            if (tile is City city)
            {
                city.Government = new History.Governments.CityGovernment(city);
                city.Government.Election();
                ((Person)city.Founder).AddTitle(typeof(CityFounder), new object[] { city });
            }
        }
    }
}
