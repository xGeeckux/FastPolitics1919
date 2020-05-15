using FastPolitics1919.Common;
using FastPolitics1919.Gfx;
using FastPolitics1919.History.Ranks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FastPolitics1919.History.Titles
{
    public class MilitaryRankTitle : Title
    {
        public MilitaryRank Rank { get; set; }
        public override BitmapImage Image => Rank.Icon;

        public MilitaryRankTitle(MilitaryRank rank)
        {
            Rank = rank;
            Name = Rank.Name;
            TitleValue = Rank.ID + 100;
        }
    }
}
