using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCreationManager : MonoBehaviour
{
    // Private and readonly text and buttons
    [SerializeField] private Text CharacterText;
    [SerializeField] private Text StatText;
    [SerializeField] private Text HPLevelText;
    [SerializeField] private Button[] buttons_plus;
    [SerializeField] private Button[] buttons_minus;

    // Variables for values
    private string c_name;
    private Gender c_gender;
    private Race c_race;
    private Class c_class;
    private Alignment c_alignment;

    // Variables for stats
    // 0 = Strength, 1 = Dexterity, 2 = Intelligence,
    // 3 = Wisdom, 4 = Constitution, 5 = Charisma
    private int statsRemaining = 15;
    private int[] stats = new int[6];
    private int[] baseStats = new int[6];

    // Variable for which of the values that have been selected
    // 0 = name, 1 = gender, 2 = race, 3 = class, 4 = alignment
    private bool[] selected = new bool[5];

    // Variable to check so several players aren't created
    private bool playerCreated = false;

    /// <summary>
    /// SETTING VALUES
    /// 
    /// Checking for when values are 0 as the first option for all types are
    /// to "please choose", therefore value - 1 is actually the first option
    /// Always calls UpdateCharacter to fix text, resetting stats etc.
    /// </summary>
    public void SetName(string name)
    {
        if (name != "")
        {
            c_name = name;
            selected[0] = true;
        }
        else
        {
            selected[0] = false;
        }
        UpdateCharacter();
    }

    public void SetGender(int value)
    {
        if (value != 0)
        {
            c_gender = (Gender)(value - 1);
            selected[1] = true;
        }
        else
        {
            selected[1] = false;
        }
        UpdateCharacter();
    }

    public void SetRace(int value)
    {
        if (value != 0)
        {
            c_race = (Race)(value - 1);
            selected[2] = true;
        }
        else
        {
            selected[2] = false;
        }
        UpdateCharacter();
    }

    public void SetClass(int value)
    {
        if (value != 0)
        {
            c_class = (Class)(value - 1);
            selected[3] = true;
        }
        else
        {
            selected[3] = false;
        }
        UpdateCharacter();
    }

    public void SetAlignment(int value)
    {
        if (value != 0)
        {
            c_alignment = (Alignment)(value - 1);
            selected[4] = true;
        }
        else
        {
            selected[4] = false;
        }
        UpdateCharacter();
    }

    /// <summary>
    /// RESETS AND UPDATES CHARACTER
    /// 
    /// First fixes the text, then resets all stats. Then updates the stats depending on chosen values.
    /// The base stats are then set to act as a limit and finally the stat points are awarded and the
    /// text and buttons are updated before changing any values
    /// </summary>
    private void UpdateCharacter()
    {
        // ABSTRACTION
        if (AllSelected())
        {
            // Update the text for the character
            CharacterText.text = $"{c_name}: A {c_gender} {c_race} {c_class} ({c_alignment})";

            // Reset all stats to 8
            ResetAllStats();

            // Changes stats depending on race and class
            UpdateAllStats();
            
            // Decides the base stats
            for(int i = 0; i < stats.Length; i++)
            {
                baseStats[i] = stats[i];
            }

            // Resets points remaining
            statsRemaining = 15;

            // Updates the text and buttons
            UpdateStatText();
            UpdateButtonsActivated();
        }
        else
        {
            CharacterText.text = "";
        }
    }

    private bool AllSelected()
    {
        for(int i = 0; i < selected.Length; i++)
        {
            if (selected[i] == false)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// STATS
    /// 
    /// Handles resetting, as well as updating depending on race and class
    /// </summary>

    private void ResetAllStats()
    {
        for (int i = 0; i < stats.Length; i++)
        {
            stats[i] = 8;
        }
    }

    private void UpdateAllStats()
    {
        switch (c_race)
        {
            case Race.Human: // Charismatic and moderate strength, lower hp
                UpdateOneStat(1, 0, 1, 0, -1, 3);
                break;
            case Race.Elf: // Smart and fast, weaker and less liked
                UpdateOneStat(-1, 2, 3, 2, -1, -1);
                break;
            case Race.Half_Elf: // Fast and smart, much less liked
                UpdateOneStat(0, 2, 2, 2, 0, -2);
                break;
            case Race.Dwarf: // Strong but stupid
                UpdateOneStat(2, 1, -1, -1, 2, 1);
                break;
            case Race.Gnome: // Very fast but very weak
                UpdateOneStat(0, 3, 1, 2, -2, 0);
                break;
            case Race.Halfling: // Overall good but ugly
                UpdateOneStat(2, 1, 0, 1, 2, -2);
                break;
        }

        switch (c_class)
        {
            case Class.Fighter: // A brute
                UpdateOneStat(2, -1, 0, 0, 2, 0);
                break;
            case Class.Ranger: // Fast and strong
                UpdateOneStat(2, 2, 0, -1, 0, 0);
                break;
            case Class.Paladin: // Strong and liked
                UpdateOneStat(1, 1, 1, -1, 0, 1);
                break;
            case Class.Mage: // Intelligent but weaker
                UpdateOneStat(0, 0, 3, 1, -1, 0);
                break;
            case Class.Cleric: // Wiser but weaker
                UpdateOneStat(0, 0, 1, 3, -1, 0);
                break;
            case Class.Thief: // Much faster and more charismatic but weaker
                UpdateOneStat(0, 3, 1, 0, -1, 1);
                break;
        }
    }

    private void UpdateOneStat(int a, int b, int c, int d, int e, int f)
    {
        stats[0] += a; // STR
        stats[1] += b; // DEX
        stats[2] += c; // INT
        stats[3] += d; // WIS
        stats[4] += e; // CON
        stats[5] += f; // CHA
    }

    /// <summary>
    /// BUTTONS AND TEXT
    /// 
    /// Handles [+] and [-] buttons, updates all text for the buttons and
    /// checks so buttons are only active when they should be active
    /// </summary>
    public void IncreaseStat(int stat)
    {
        stats[stat]++;
        statsRemaining--;
        UpdateStatText();
        UpdateButtonsActivated();
    }

    public void DecreaseStat(int stat)
    {
        stats[stat]--;
        statsRemaining++;
        UpdateStatText();
        UpdateButtonsActivated();
    }

    private void UpdateStatText()
    {
        StatText.text = $"{stats[0]}\n{stats[1]}\n{stats[2]}\n{stats[3]}\n{stats[4]}\n{stats[5]}";
        HPLevelText.text = $"HP: {stats[4] * 2}\nLevel: 1";
    }

    private void UpdateButtonsActivated()
    {
        bool activateButton;
        
        if (statsRemaining <= 0)
            activateButton = false;
        else
            activateButton = true;

        for (int i = 0; i < buttons_plus.Length; i++)
        {
            buttons_plus[i].gameObject.SetActive(activateButton);
            
            if (stats[i] == baseStats[i])
            {
                buttons_minus[i].gameObject.SetActive(false);
            } else
            {
                buttons_minus[i].gameObject.SetActive(true);
            }
        }
    } 

    /// <summary>
    /// CREATING PLAYER
    /// 
    /// Creates a GameObject, adds the component Player Character and updates the start values
    /// </summary>
    public void CreateCharacter()
    {
        if (AllSelected() && statsRemaining == 0 && !playerCreated)
        {
            playerCreated = true;
            GameObject Player = new GameObject();
            Player.AddComponent<PlayerCharacter>();
            Player.GetComponent<PlayerCharacter>().StartValues(c_name, c_gender, c_race, c_class, c_alignment, stats);
        }
        else if (playerCreated)
        {
            Debug.Log("A player has already been created.");
        }
        else
        {
            Debug.Log("You must choose all aspects of your character and spend all points!");
        }
    }
}
