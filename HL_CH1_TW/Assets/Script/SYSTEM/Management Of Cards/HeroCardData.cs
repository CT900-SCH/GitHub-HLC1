using System;
using UnityEngine;

public enum HeroCardRole
{
    Vanguard,
    Tank,
    Ranger
}

public enum HeroCardRace
{
    Human,
    Elven
}

public enum HeroCardName
{
    BenTheSniper,
    TanyaTheFighter,
    MatthewTheThief
}

public enum HeroCardClass
{
    Gunner,
    Fighter,
    Ranger,
    Barbarian = 3,
    Knight = 4,
    Mage = 5,
    Priest = 6,
    Mech = 7
}

public enum HeroWeaponClass
{
    Swordsman,
    Axeman,
    Spearman,
    Marksman,
    Archer,
    Fighter,
    Assassin
}

public enum HeroWeaponClassLevel
{
    Level1 = 1,
    Level2 = 2,
    Level3 = 3,
    Level4 = 4
}

public enum HeroWeaponClassIconAmount
{
    None = 0,
    One = 1,
    Two = 2,
    Three = 3,
    Four = 4
}

[CreateAssetMenu(
    fileName = "HeroCard_New",
    menuName = "Heroes Landing/Card Data/Hero Card"
)]
public class HeroCardData : ScriptableObject
{
    [Header("Card Information")]

    public string cardID;
    public string illustratorName;
    public CardColor cardColor;
    public HeroCardRole role;

    [Header("Card Images")]

    public Sprite cardArtwork;
    public Sprite expansionIcon;

    [Header("Battlefield Information")]

    [Min(0)]
    public int cardCost;

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
    public HeroCardRace race;
    public HeroCardName characterName;
    public HeroCardClass characterClass;

    [Header("Weapon Information")]

    public HeroWeaponClass weaponClass;

    [Tooltip("Total number of white weapon boxes.")]
    public HeroWeaponClassLevel weaponClassLevel =
        HeroWeaponClassLevel.Level1;

    [Tooltip("How many boxes contain weapon icons.")]
    public HeroWeaponClassIconAmount weaponClassIconAmount =
        HeroWeaponClassIconAmount.None;

    [HideInInspector]
    public HeroWeaponClass[] selectedWeaponBoxIcons;

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
            (HeroWeaponClassIconAmount)iconBoxes;

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