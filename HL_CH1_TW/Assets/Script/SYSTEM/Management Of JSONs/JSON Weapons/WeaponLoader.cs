using UnityEngine;
using System.Collections.Generic;



public enum TypeOfWeapon
{
    Melee,
    Range
}

public enum WeaponRarity
{
    Common,
    Rare,
    Epic,
    Legendary,
    Mystical,
}

[System.Serializable]
public class Weapon
{
    public TypeOfWeapon weaponType;
    public string spritePath;
    public string prefabPath;
    public string weaponName;
    public int damage;
    public float speed;
    public float cooldownDuration;
    public int pierce;
    [TextArea(2, 10)]
    public string description;

    public int droprate = 100;
    public WeaponRarity rarity = WeaponRarity.Common;

    [System.NonSerialized] public Sprite loadedSprite;
    [System.NonSerialized] public GameObject loadedPrefab;
}

[System.Serializable]
public class WeaponList
{
    public List<Weapon> weapons;
}

public class WeaponLoader : SimpleSingleton<WeaponLoader>
{
    public WeaponList myWeaponList = new WeaponList();
    void Start()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("weapons");
        if (jsonFile != null)
        {
            myWeaponList = JsonUtility.FromJson<WeaponList>(jsonFile.text);
        }
        else
        {
            Debug.LogError("Could not find player.json in Resources folder.");
        }
    }
}
