namespace Eric.CharacterStats
{
    public enum StatModType
    {
        Flat = 100,
        PercentAdd = 200,
        PercentMult = 300,
    }

    public class StatModifier
    {
        // The value, the type and order of the modifier
        public readonly float Value;
        public readonly StatModType Type;
        public readonly int Order;

        // Using an object to keep track where modifiers came from
        // Also allows to remove based on an object instead of removing each modifier separately
        public readonly object Source;

        // Construktor
        public StatModifier(float value, StatModType type, int order, object source)
        {
            Value = value;
            Type = type;
            Order = order;
            Source = source;
        }

        // Extra constructor to only pass two or three parameters. value and type are required,
        // order and source are optional
        // The type works as a default, by handling Flat first and Percent later
        // The source works as a way to keep track from where modifiers came from
        // Can use the constructor above if modifiers are to be calculated in a different order
        public StatModifier(float value, StatModType type) : this(value, type, (int)type, null) { }

        public StatModifier(float value, StatModType type, int order) : this(value, type, order, null) { }

        public StatModifier(float value, StatModType type, object source) : this(value, type, (int)type, source) { }
    }
}