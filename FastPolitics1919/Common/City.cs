using FastPolitics1919.Data.Common;
using FastPolitics1919.History.Governments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FastPolitics1919.Common
{
    [Serializable]
    public class City : Tile
    {
        #region Government
        public CityGovernment Government { get; set; }
        #endregion

        public override BitmapImage Icon => Gfx.Images.IconCity;
        public override BitmapImage BackgroundImage => Gfx.Images.IconBackgroundCity;
    }
}
