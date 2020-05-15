using FastPolitics1919.Common;
using FastPolitics1919.Gfx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FastPolitics1919.History.Titles
{
    public class PartyFounder : Title
    {
        public Party Party { get; set; }
        public override BitmapImage Image => Images.IconQuestionmark;

        public PartyFounder(Party party)
        {
            Name = "Vorsitzender";
            Party = party;
            TitleValue = 3;
        }
    }
}
