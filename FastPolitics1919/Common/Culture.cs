using FastPolitics1919.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPolitics1919.Common
{
    public class Culture : GameObject
    {
        public static int Index = 0;

        //- Constructor
        public Culture(string name)
        {
            Name = name;
            Index++;
            ID = Index;
        }
    }
}
