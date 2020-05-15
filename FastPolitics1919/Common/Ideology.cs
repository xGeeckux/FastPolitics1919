using FastPolitics1919.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPolitics1919.Common
{
    [Serializable]
    public abstract class Ideology : GameObject
    {
        public new abstract string Name { get; }
        public abstract string Color { get; }

        public class Amount
        {
            public Ideology Ideology { get; set; }
            public double Proportion { get; set; }

            public Amount(Ideology ideology, double proportion)
            {
                Ideology = ideology;
                Proportion = proportion;
            }
        }
        public class Slot
        {
            public Ideology Ideology { get; set; }

            public void SetIdeology(Ideology ideology)
            {
                Ideology = ideology;
            }
            public void SetIdeology(int id)
            {
                Ideology = Engine.Game.FindIdeology(id);
            }
        }

        public static Ideology Get(Ideologies ideology)
        {
            return Engine.Game.FindIdeology((int)ideology);
        }
    }
    public enum Ideologies : int
    {
        Conservative = 1,
        Fascist = 4,
        Liberal = 5,
        Democrat = 6,
        Communist = 11
    }
}
