using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Eric.CharacterStats;

// INHERITANCE
public class PlayerCharacter : Character
{
    // ENCAPSULATION
    public string characterName { get; private set; }
    public Gender charGender { get; private set; }
    public Race charRace { get; private set; }
    public Class charClass { get; private set; }
    public Alignment charAlignment { get; private set; }

    public CharacterStat Strength { get; private set; }
    public CharacterStat Dexterity { get; private set; }
    public CharacterStat Intelligence { get; private set; }
    public CharacterStat Wisdom { get; private set; }
    public CharacterStat Constitution { get; private set; }
    public CharacterStat Charisma { get; private set; }

    public void StartValues(string name, Gender c_gender, Race c_race, Class c_class, Alignment c_alignment, int[] stats)
    {
        characterName = name;
        gameObject.name = name;
        charGender = c_gender;
        charRace = c_race;
        charClass = c_class;
        charAlignment = c_alignment;

        Strength.BaseValue = stats[0];
        Dexterity.BaseValue = stats[1];
        Intelligence.BaseValue = stats[2];
        Wisdom.BaseValue = stats[3];
        Constitution.BaseValue = stats[4];
        Charisma.BaseValue = stats[5];

        maxHp = (int) Constitution.Value * 2;
        currentHp = maxHp;
    }

    // POLYMORPHISM
    protected override void Dead()
    {
        base.Dead();
        Debug.Log($"The character {characterName} has died!");
    }
}

// Example of items
/* public class Item
{
    public void Equip(PlayerCharacter c)
    {
        c.Strength.AddModifier(new StatModifier(10, StatModType.Flat, this));
        c.Strength.AddModifier(new StatModifier(0.1f, StatModType.PercentMult, this));
    }

    public void Unequip(PlayerCharacter c)
    {
        c.Strength.RemoveAllModifiersFromSource(this);
    }
}
*/