using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "WeaponScriptableObject", menuName = "ScriptableObjects/Weapon")]
public class WeaponScriptableObject : ScriptableObject
{
    [Header("Weapon Stats")]
    
    [SerializeField] 
    string weaponName;
    public string WeaponName { get => weaponName; private set => weaponName = value; }

    [SerializeField] 
    TypeOfWeapon weaponType;
    public TypeOfWeapon WeaponType { get => weaponType; private set => weaponType = value; }

    [SerializeField]
    Sprite sprite;
    public Sprite Sprite { get => sprite; private set => sprite = value; }

    [SerializeField]
    GameObject prefab;
    public GameObject Prefab { get => prefab; private set => prefab = value; }

    // Based Stats For Weapons
    [SerializeField]
    float damage;
    public float  Damage { get => damage; private set => damage = value; }

    [SerializeField]
    float speed;
    public float Speed { get => speed; private set => speed = value; }

    [SerializeField]
    float cooldownDuration;
    public float CoolDownDuration { get => cooldownDuration; private set => cooldownDuration = value; }

    [SerializeField]
    int pierce;
    public int Pierce { get => pierce; private set => pierce = value; }

    [SerializeField]
    [TextArea(3, 10)]
    string description;
    public string Description { get => description; private set => description = value; }

    public void Initialize(
    string name,
    TypeOfWeapon type,
    Sprite sprite,
    GameObject prefab,
    float damage,
    float speed,
    float cooldown,
    int pierce,
    string description)
    {
        this.weaponName = name;
        this.weaponType = type;
        this.sprite = sprite;
        this.prefab = prefab;
        this.damage = damage;
        this.speed = speed;
        this.cooldownDuration = cooldown;
        this.pierce = pierce;
        this.description = description;
    }
}
