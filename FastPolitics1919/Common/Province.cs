using FastPolitics1919.Data.Common;
using FastPolitics1919.History.Governments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPolitics1919.Common
{
    [Serializable]
    public class Province : MapObject
    {
        #region Owners
        public Country Owner { get; set; }
        public Country Controller { get; set; }
        public List<Country> Claims { get; set; }
        public void SetOwner(Country country)
        {
            Owner = country;
            foreach (Tile tile in Tiles)
            {
                tile.SetController(Owner);
            }
        }
        #endregion

        #region Cities
        public List<Tile> Tiles
        {
            get
            {
                List<Tile> local = new List<Tile>();
                foreach (Tile tile in Engine.Game.Tiles.Get())
                {
                    if (tile.Owner == this)
                        local.Add(tile);
                }
                return local;
            }
        }
        public List<City> Cities
        {
            get
            {
                List<City> local = new List<City>();
                foreach (Tile tile in Engine.Game.Tiles.Get())
                {
                    if (tile is City city && city.Owner == this)
                        local.Add(city);
                }
                return local;
            }
        }
        public int CityCount => Cities.Count;
        #endregion

        #region Government
        public ProvinceGovernment Government { get; set; }
        #endregion
    }
}
