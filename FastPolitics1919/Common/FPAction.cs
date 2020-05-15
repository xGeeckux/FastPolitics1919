using FastPolitics1919.Gfx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace FastPolitics1919.Common
{
    public abstract class FPAction
    {
        public abstract string Name { get; }
        public virtual bool ForeignInteraction => true;

        public Person First { get; set; }
        public Person Secound { get; set; }

        public int IndexPosition { get; set; }

        public virtual BitmapImage Icon => Images.IconQuestionmark;
        public virtual string HexColor => "#FF175EB4";

        public FPAction(Person first, Person target)
        {
            IndexPosition = 0;
            First = first;
            Secound = target;
            Prequisition();
        }

        public abstract void Prequisition();
        public virtual bool Condition() => ForeignInteraction ? First != Secound : First == Secound;
        public abstract void Effect();

        public virtual void Click(object sender, RoutedEventArgs e)
        {
            Effect();
        }
    }
}
