using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Gender
{
    Male,
    Female,
}

public enum Race
{
    Human,
    Elf,
    Half_Elf,
    Dwarf,
    Gnome,
    Halfling,
}

public enum Class
{
    Fighter,
    Ranger,
    Paladin,
    Mage,
    Cleric,
    Thief,
}

public enum Alignment
{
    Lawful_Good,
    Lawful_Neutral,
    Lawful_Bad,
    Neutral_Good,
    True_Neutral,
    Neutral_Bad,
    Chaotic_Good,
    Chaotic_Neutral,
    Chaotic_Bad,
}

public enum DamageType
{
    Physical,
    Magic,
}

 public abstract class Character : MonoBehaviour
{
    // Base stats
    [SerializeField] protected int maxHp;
    [SerializeField] protected int currentHp;
    [SerializeField] protected int armor_Physical = 0;
    [SerializeField] protected int armor_Magic = 0;
    [SerializeField] protected int attack;
    [SerializeField] DamageType attackType;

    // Resistances
    [SerializeField] protected int poisonResistance = 50;
    [SerializeField] protected List<int> poisonTurns = new List<int>();

    [SerializeField] protected int charmResistance = 50;
    [SerializeField] protected bool charmed = false;

    [SerializeField] protected int stunResistance = 50;
    [SerializeField] protected bool stunned = false;

    // Attack another character with a certain value of attack
    public virtual void AttackTarget(Character target)
    {
        target.TakeDamage(attack, attackType);
    }

    // Take damage from other enemies or events
    public virtual void TakeDamage(int attack, DamageType type)
    {
        int damage = attack;
        
        if (type == DamageType.Physical)
        {
            // Sets the physical damage to 0 if the armor is higher than the attack
            damage = (attack - armor_Physical > 0) ? (attack - armor_Physical) : 0;
            Debug.Log($"The attack is a physical attack with the damage {attack} and the armor is {armor_Physical}, which results in the damage {damage}");
        } else if (type == DamageType.Magic)
        {
            // Sets the magic damage to 0 if the magic armor is higher than the attack
            damage = (attack - armor_Magic > 0) ? (attack - armor_Magic) : 0;
            Debug.Log($"The attack is a physical attack with the damage {attack} and the armor is {armor_Magic}, which results in the damage {damage}");
        }

        // Removes HP and kills the character if HP is 0 or lower
        currentHp -= damage;
        if (currentHp <= 0)
        {
            Dead();
        }
    }

    // Add poison for a certain number of turns
    public virtual void Poison(int poisonChance, int turns, int damage)
    {
        if (Random.Range(0, poisonChance) > poisonResistance)
        {
            for(int i = 0; i < turns; i++)
            {
                poisonTurns[i] += damage; 
            }
        }
    }

    // Charm the character
    public virtual void Charm(int charmChance)
    {
        if (Random.Range(0, charmChance) > charmResistance)
        {
            charmed = true;
        }
    }

    // Charm the character
    public virtual void Stun(int stunChance)
    {
        if (Random.Range(0, stunChance) > stunResistance)
        {
            stunned = true;
        }
    }

    // Heal the character
    public virtual void Heal(int heal)
    {
        // Preventing overhealing
        if (currentHp + heal >= maxHp)
        {
            currentHp = maxHp;
        } else
        {
            currentHp += heal;
        }
    }

    // What happens when the character dies
    protected virtual void Dead()
    {
        gameObject.SetActive(false);
    }
}
