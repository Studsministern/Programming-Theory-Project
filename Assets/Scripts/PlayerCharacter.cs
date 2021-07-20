using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    public string characterName { get; private set; }
    public Gender charGender { get; private set; }
    public Race charRace { get; private set; }
    public Class charClass { get; private set; }
    public Alignment charAlignment { get; private set; }

    public int strength { get; private set; }
    public int dexterity { get; private set; }
    public int intelligence { get; private set; }
    public int wisdom { get; private set; }
    public int constitution { get; private set; }
    public int charisma { get; private set; }

    public void StartValues(string name, Gender c_gender, Race c_race, Class c_class, Alignment c_alignment, int[] stats)
    {
        characterName = name;
        gameObject.name = name;
        charGender = c_gender;
        charRace = c_race;
        charClass = c_class;
        charAlignment = c_alignment;

        strength = stats[0];
        dexterity = stats[1];
        intelligence = stats[2];
        wisdom = stats[3];
        constitution = stats[4];
        charisma = stats[5];

        maxHp = constitution * 2;
        currentHp = maxHp;
    }

    protected override void Dead()
    {
        base.Dead();
        Debug.Log($"The character {characterName} has died!");
    }
}
