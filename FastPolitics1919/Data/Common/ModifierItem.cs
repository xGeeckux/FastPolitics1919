
using System;

namespace FastPolitics1919.Data.Common
{
    public struct ModifierItem
    {
        private ModifierProperty Property => Modifier.List[(int)enum_modifier];
        public string Name => Property.Name;
        public double Value { get; set; }

        public bool IsFraction => Property.IsFraction;
        public RelativeValue RelativeValue => Property.RelativeValue;

        private Modifiers enum_modifier { get; set; }

        public ModifierItem(Modifiers modifier, double modifier_value)
        {
            enum_modifier = modifier;
            Value = modifier_value;
        }

        public static ModifierItem operator +(ModifierItem a, ModifierItem b)
        {
            return new ModifierItem(a.enum_modifier, a.Value + b.Value);
        }

        public bool Is(Modifiers modifier) => Property.ID == modifier;

        public override string ToString() => Name + ": " + GetValue();

        public string GetValue()
        {
            if (IsFraction)
                Value = Value * 100;
            if (Value > 0)
                return "+" + Math.Round(Value, 2) + (IsFraction ? " %" : "");
            if (Value == 0)
                return Math.Round(Value, 2).ToString() + (IsFraction ? " %" : "");
            if (Value < 0)
                return "-" + Math.Round(Value, 2).ToString().Substring(1, Math.Round(Value, 2).ToString().Length - 1) + (IsFraction ? " %" : "");
            return null;
        }
    }
    public struct SingleModifierItem
    {
        public string Name => Modifier.Name;
        public double Value => Item.Value;

        public ModifierItem Item { get; set; }
        public Modifier Modifier { get; set; }

        public SingleModifierItem(Modifier modifier, ModifierItem item)
        {
            Item = item;
            Modifier = modifier;
        }
    }
}
