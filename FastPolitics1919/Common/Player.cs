using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPolitics1919.Common
{
    public class Player : Person
    {
        //- Action-Points (AP)
        public int CurActionPoints { get; set; }
        public int MaxActionPoints { get; set; } = 10;
    }
}
