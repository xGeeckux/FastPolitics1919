
using System.Collections.Generic;

namespace FastPolitics1919.Data.Common
{
    public class Modifier
    {
        public string Name { get; set; }
        public List<ModifierItem> Items { get; set; }

        public Modifier(string name)
        {
            Name = name;
            Items = new List<ModifierItem>();
        }

        internal static ModifierProperty[] List => PList;
        private static ModifierProperty[] PList { get; set; }
        public static void LoadModifiers()
        {
            List<ModifierProperty> properties = new List<ModifierProperty>();

            properties.Add(new ModifierProperty("Stadt Gründungs Modifier", true, RelativeValue.Negativ));

            PList = properties.ToArray();
        }
    }
    public enum Modifiers : int
    {
        LocalCityFoundModifier
    }
}
