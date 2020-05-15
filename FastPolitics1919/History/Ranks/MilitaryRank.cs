using FastPolitics1919.Common;
using FastPolitics1919.Gfx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FastPolitics1919.History.Ranks
{
    public class MilitaryRank : Rank
    {
        public string IconPath => ID + "_icon";
        public BitmapImage Icon => Images.FromPath(Images.military_ranks + IconPath);

        public RankTypes Type { get; set; }

        public MilitaryRank(int id, string name, RankTypes type)
        {
            ID = id;
            Name = name;
            Type = type;
        }
    }
}
