using FastPolitics1919.Data.Common;
using FastPolitics1919.History.Governments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FastPolitics1919.Common
{
    [Serializable]
    public class Country : MapObject
    {
        #region Colors
        public string RGBColor { get; set; }
        public BitmapImage Flag { get; set; }
        #endregion

        #region Provinces
        public int CapitalID { get; set; }
        public City Capital => Engine.Game.FindCity(CapitalID);
        public Province CapitalProvince => Capital.Owner;
        public List<Province> Provinces
        {
            get
            {
                List<Province> provinces = new List<Province>();
                foreach (Province province in Engine.Game.Provinces.Get())
                {
                    if (province.Owner == this)
                        provinces.Add(province);
                }
                return provinces;
            }
        }
        public int ProvinceCount => Provinces.Count;
        #endregion

        #region Army
        public Army Army { get; set; }
        #endregion

        #region Government
        public CountryGovernment Government { get; set; }
        #endregion
    }
}
