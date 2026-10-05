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
    Ranger
}

public enum HeroWeaponClass
{
    Swordsman,
    Axeman,
    Spearman,
    Marksman,
    Archer,
    Fighter,
    Rogue,
}

public enum HeroWeaponClassLevel
{
    Level1 = 1,
    Level2 = 2,
    Level3 = 3,
    Level4 = 4,
    Level5 = 5
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

    [Header("Battlefield Information")]
    public int cardCost;

    public HeroCardRole role;

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

    public HeroWeaponClassLevel weaponClassLevel =
        HeroWeaponClassLevel.Level1;

    [Tooltip("The white boxes representing the Weapon Class Level.")]
    public Sprite[] weaponClassLevelBoxSprites;

    [Tooltip("The weapon symbols placed over the white boxes.")]
    public Sprite[] weaponClassIconSprites;

    [Header("Card Writing")]

    [TextArea(4, 10)]
    public string description;

    [TextArea(3, 8)]
    public string flavorText;

    private void OnValidate()
    {
        int requiredBoxes = (int)weaponClassLevel;

        ResizeSpriteArray(
            ref weaponClassLevelBoxSprites,
            requiredBoxes
        );

        ResizeSpriteArray(
            ref weaponClassIconSprites,
            requiredBoxes
        );
    }

    private void ResizeSpriteArray(
        ref Sprite[] spriteArray,
        int requiredSize
    )
    {
        if (spriteArray == null)
        {
            spriteArray = new Sprite[requiredSize];
            return;
        }

        if (spriteArray.Length != requiredSize)
        {
            Array.Resize(
                ref spriteArray,
                requiredSize
            );
        }
    }
}