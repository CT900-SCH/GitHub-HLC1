using System;
using UnityEngine;

public enum PlayerCardRole
{
    Vanguard,
    Tanker,
    Ranger
}

public enum PlayerCardRace
{
    Human,
    Elven
}

public enum PlayerCardName
{
    RustyTheGreat
}

public enum PlayerCardClass
{
    Barbarian,
    Knight,
    Fighter,
    Mage,
    Priest,
    Ranger,
    Gunner,
    Mech
}

public enum PlayerWeaponClass
{
    Swordsman,
    Axeman,
    Spearman,
    Marksman,
    Archer,
    Fighter,
    Assassin
}

public enum WeaponClassLevel
{
    Level1 = 1,
    Level2 = 2,
    Level3 = 3,
    Level4 = 4
}

public enum WeaponClassIconAmount
{
    None = 0,
    One = 1,
    Two = 2,
    Three = 3,
    Four = 4
}

[CreateAssetMenu(
    fileName = "PlayerCard_New",
    menuName = "Heroes Landing/Card Data/Player Card"
)]
public class PlayerCardData : ScriptableObject
{
    [Header("Card Information")]

    public string cardID;
    public string illustratorName;
    public CardColor cardColor;
    public PlayerCardRole role;

    [Header("Card Images")]
    [Tooltip("The main character artwork displayed on the card.")]
    public Sprite cardArtwork;

    [Tooltip("The expansion-set icon displayed on the card.")]
    public Sprite expansionIcon;

    [Header("Character Statistics")]

    [Min(0)]
    public int carry;

    [Min(0)]
    public int moveSpeed;

    [Min(0)]
    public int attack;

    [Min(0)]
    public int defense;

    [Header("Character Identity")]

    public string title;
    public PlayerCardRace race;
    public PlayerCardName characterName;
    public PlayerCardClass characterClass;

    [Header("Weapon Information")]

    [Tooltip("The Player's main weapon class.")]
    public PlayerWeaponClass weaponClass;

    [Tooltip("Total number of white weapon boxes.")]
    public WeaponClassLevel weaponClassLevel =
        WeaponClassLevel.Level4;

    [Tooltip("How many boxes contain weapon icons.")]
    public WeaponClassIconAmount weaponClassIconAmount =
        WeaponClassIconAmount.None;

    /*
     * Each filled weapon box can use a different weapon icon.
     * The custom editor displays these as dropdown selections.
     */
    [HideInInspector]
    public PlayerWeaponClass[] selectedWeaponBoxIcons;

    /*
     * Automatically assigned by PlayerCardDataEditor.
     */
    [HideInInspector]
    public Sprite roleIcon;

    [HideInInspector]
    public Sprite classIcon;

    [HideInInspector]
    public Sprite[] weaponClassLevelBoxSprites;

    [HideInInspector]
    public Sprite[] weaponClassIconSprites;

    [Header("Card Writing")]

    [TextArea(4, 10)]
    public string description;

    [TextArea(3, 8)]
    public string flavorText;

    public void EnsureWeaponArrays()
    {
        int totalBoxes = Mathf.Clamp(
            (int)weaponClassLevel,
            1,
            4
        );

        int iconBoxes = Mathf.Clamp(
            (int)weaponClassIconAmount,
            0,
            totalBoxes
        );

        weaponClassIconAmount =
            (WeaponClassIconAmount)iconBoxes;

        Array.Resize(
            ref selectedWeaponBoxIcons,
            totalBoxes
        );

        Array.Resize(
            ref weaponClassLevelBoxSprites,
            totalBoxes
        );

        Array.Resize(
            ref weaponClassIconSprites,
            totalBoxes
        );
    }

    private void OnValidate()
    {
        EnsureWeaponArrays();
    }
}