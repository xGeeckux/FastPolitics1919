
namespace FastPolitics1919.Data.Common
{
    public struct ModifierProperty
    {
        private static int Index = 0;
        public string Name { get; set; }
        public Modifiers ID => (Modifiers)LocalID;
        private int LocalID { get; set; }

        public bool IsFraction { get; set; }
        public RelativeValue RelativeValue { get; set; }

        public ModifierProperty(string name, bool fraction, RelativeValue value)
        {
            Name = name;
            LocalID = Index++;
            IsFraction = fraction;
            RelativeValue = value;
        }
    }
    public enum RelativeValue
    {
        Positiv, Negativ
    }
}
