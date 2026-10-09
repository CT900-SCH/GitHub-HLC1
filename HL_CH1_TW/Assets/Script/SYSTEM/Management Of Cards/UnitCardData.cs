using System;
using UnityEngine;

public enum UnitCardRole
{
    Vanguard,
    Tank,
    Ranger
}

public enum UnitCardRace
{
    Human,
    Elven
}

public enum UnitCardClass
{
    Gunner,
    Fighter,
    Ranger,
    Barbarian,
    Knight,
    Mage,
    Priest,
    Mech
}

public enum UnitWeaponClass
{
    Swordsman,
    Axeman,
    Spearman,
    Marksman,
    Archer,
    Fighter,
    Assassin
}

public enum UnitWeaponClassLevel
{
    Level1 = 1,
    Level2 = 2,
    Level3 = 3,
    Level4 = 4
}

public enum UnitWeaponClassIconAmount
{
    None = 0,
    One = 1,
    Two = 2,
    Three = 3,
    Four = 4
}

[CreateAssetMenu(
    fileName = "UnitCard_New",
    menuName = "Heroes Landing/Card Data/Unit Card"
)]
public class UnitCardData : ScriptableObject
{
    [Header("Card Information")]

    public string cardID;

    public string illustratorName;

    public CardColor cardColor;

    public Sprite cardArtwork;

    public Sprite illustrationArtwork;

    public Sprite expansionIcon;

    [Header("Battlefield Information")]

    [Min(0)]
    public int cardCost;

    public UnitCardRole role;

    [Min(0)]
    public int size;

    [Min(0)]
    public int moveSpeed;

    [Min(0)]
    public int attack;

    [Min(0)]
    public int defense;

    [Header("Character Identity")]

    public string title;

    public UnitCardRace race;

    public UnitCardClass characterClass;

    [Header("Weapon Information")]

    [Tooltip("The Unit's main weapon class.")]
    public UnitWeaponClass weaponClass;

    [Tooltip("Total number of white weapon boxes.")]
    public UnitWeaponClassLevel weaponClassLevel =
        UnitWeaponClassLevel.Level4;

    [Tooltip("How many weapon boxes contain icons.")]
    public UnitWeaponClassIconAmount weaponClassIconAmount =
        UnitWeaponClassIconAmount.None;

    [Tooltip(
        "The weapon class icon selected for each filled box."
    )]
    [HideInInspector]
    public UnitWeaponClass[] selectedWeaponBoxIcons;

    [Header("Automatically Assigned Icons")]

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
            (UnitWeaponClassIconAmount)iconBoxes;

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