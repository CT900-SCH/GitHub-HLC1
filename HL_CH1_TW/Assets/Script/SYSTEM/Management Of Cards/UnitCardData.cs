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

public enum UnitCardName
{
    BenTheSniper,
    TanyaTheFighter,
    MatthewTheThief
}

public enum UnitCardClass
{
    Gunner,
    Fighter,
    Ranger,
    Knight
}

public enum UnitWeaponClass
{
    Swordsman,
    Axeman,
    Spearman,
    Marksman,
    Archer,
    Fighter,
    Rogue,
}

[CreateAssetMenu(
    fileName = "UnitCard_New",
    menuName = "Heroes Landing/Card Data/Unit Card"
)]
public class UnitCardData : ScriptableObject
{
    [Header("Card Information")]

    [Min(0)]
    public int cardID;

    public string illustratorName;

    public CardColor cardColor;

    [Header("Battlefield Information")]
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

    public UnitWeaponClass weaponClass;

    [Header("Card Writing")]

    [TextArea(4, 10)]
    public string description;

    [TextArea(3, 8)]
    public string flavorText;
}