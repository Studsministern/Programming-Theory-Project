using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Eric.CharacterStats
{
    // Make it possible to see characterstats in the inspector
    [Serializable]
    public class CharacterStat
    {
        // The base value for a certain stat
        public float BaseValue;

        // Encapsulation to protect the real value
        public virtual float Value
        {
            get
            {
                if (isDirty || BaseValue != _baseValue)
                {
                    _value = CalculateFinalValue();
                    _baseValue = BaseValue;
                    isDirty = false;
                }
                return _value;
            }
        }

        // _Value has to be re-calculated if isDirty is true
        protected bool isDirty = true;

        // Keeps the most recent calculation, will only be calculated if needed
        protected float _value;
        protected float _baseValue;

        // Readonly list = can assign value in constructor but cannot set to null later (for example)
        // Using a public ReadOnlyCollection to allow other scripts to view the stat modifiers without changing them
        // Needs System.Collections.ObjectModel and for StatModifiers to reference statModifiers
        // Using readonly as well prevents the StatModifiers to point to a different list
        protected readonly List<StatModifier> statModifiers;
        public readonly ReadOnlyCollection<StatModifier> StatModifiers;

        // Constructor without parameters to handle null-refence exception
        public CharacterStat()
        {
            statModifiers = new List<StatModifier>();
            StatModifiers = statModifiers.AsReadOnly();
        }

        // Constructor with parameter to handle baseValue
        public CharacterStat(float baseValue) : this()
        {
            BaseValue = baseValue;
        }

        // Add a modifier
        public virtual void AddModifier(StatModifier mod)
        {
            isDirty = true;
            statModifiers.Add(mod);
            statModifiers.Sort(CompareModifierOrder);
        }

        // Comparison function for explaining to sort that smaller order will come before larger order
        protected virtual int CompareModifierOrder(StatModifier a, StatModifier b)
        {
            if (a.Order < b.Order)
                return -1;
            else if (a.Order > b.Order)
                return 1;
            return 0;
        }

        // Remove a modifier
        public virtual bool RemoveModifier(StatModifier mod)
        {
            if (statModifiers.Remove(mod))
            {
                isDirty = true;
                return true;
            }
            return false;
        }

        // Remove based on a object
        public virtual bool RemoveAllModifiersFromSource(object source)
        {
            bool didRemove = false;

            // Goes backwards to handle removing from a list
            for (int i = statModifiers.Count - 1; i >= 0; i--)
            {
                if (statModifiers[i].Source == source)
                {
                    isDirty = true;
                    didRemove = true;
                    statModifiers.RemoveAt(i);
                }
            }

            return didRemove;
        }

        // Calculate the stat with all modifiers
        protected virtual int CalculateFinalValue()
        {
            // Temporary value
            float finalValue = BaseValue;

            // The sum of all the percentadd modifiers
            float sumPercentAdd = 0;

            // Add together all modifiers
            for (int i = 0; i < statModifiers.Count; i++)
            {
                StatModifier mod = statModifiers[i];

                // Add it the type is flat
                if (mod.Type == StatModType.Flat)
                {
                    finalValue += mod.Value;
                }
                // Calculate all percentAdd before multiplying
                else if (mod.Type == StatModType.PercentAdd)
                {
                    sumPercentAdd += mod.Value;

                    // Checks if it's the last modifier of the list or if the next modifier is of a different type
                    if (i + 1 >= statModifiers.Count || statModifiers[i + 1].Type != StatModType.PercentAdd)
                    {
                        finalValue *= 1 + sumPercentAdd;
                        sumPercentAdd = 0;
                    }
                }
                // Simply multiply if the type is PercentMult
                else if (mod.Type == StatModType.PercentMult)
                {
                    finalValue *= 1 + mod.Value;
                }

            }

            // Round the value
            // 12.0001f != 12f
            return (int)Math.Round(finalValue);
        }
    }
}