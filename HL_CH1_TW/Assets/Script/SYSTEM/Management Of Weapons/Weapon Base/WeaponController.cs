using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
///  Base script for all weapon controller
/// </summary>
public class WeaponController : SimpleSingleton<WeaponController>
{
    [Header("Weapon Stats")]
    public List<Weapon> weaponDataList = new List<Weapon>();
    private List<float> currentCooldowns = new List<float>();

    protected Player pm;

    
    protected virtual void Awake()
    {

    }
    protected virtual void Start()
    {
        pm = FindObjectOfType<Player>();

        // Initialize cooldowns based on loaded weapons
        foreach (var weapon in weaponDataList)
        {
            currentCooldowns.Add(weapon.cooldownDuration);

            // Load prefab and store
            if (!string.IsNullOrEmpty(weapon.prefabPath))
            {
                GameObject prefab = Resources.Load<GameObject>(weapon.prefabPath);
                if (prefab != null)
                    weapon.loadedPrefab = prefab;
                else
                    Debug.LogError($"Failed to load prefab from path: {weapon.prefabPath}");
            }

            // Load sprite and store
            if (!string.IsNullOrEmpty(weapon.spritePath))
            {
                Sprite spriteForIcon = Resources.Load<Sprite>(weapon.spritePath);
                if (spriteForIcon != null)
                {
                    weapon.loadedSprite = spriteForIcon;
                }

                else
                    Debug.LogError($"Could not load sprite at path: {weapon.spritePath}");
            }
        }
    }

    protected virtual void Update()
    {
        for (int i = 0; i < weaponDataList.Count; i++)
        {
            currentCooldowns[i] -= Time.deltaTime;

            if (currentCooldowns[i] <= 0f)
            {
                Attack(weaponDataList[i]);
                currentCooldowns[i] = weaponDataList[i].cooldownDuration;
            }
        }
    }

    public bool AddWeapon(Weapon newWeapon)
    {
        // Cannot have the same weapon
        if (weaponDataList.Exists(w => w.weaponName == newWeapon.weaponName))
        {
            Debug.LogWarning($"Weapon {newWeapon.weaponName} already exists.");
            return false;
        }

        // Load prefab and store
        if (!string.IsNullOrEmpty(newWeapon.prefabPath))
        {
            GameObject prefab = Resources.Load<GameObject>(newWeapon.prefabPath);
            if (prefab != null)
            {
                newWeapon.loadedPrefab = prefab;
            }
                
            else
                Debug.LogError($"Could not load prefab at path: {newWeapon.prefabPath}");
        }

        // Load sprite and store
        if (!string.IsNullOrEmpty(newWeapon.spritePath))
        {
            Sprite spriteForIcon = Resources.Load<Sprite>(newWeapon.spritePath);
            if (spriteForIcon != null)
            {
                newWeapon.loadedSprite = spriteForIcon;
            }

            else
                Debug.LogError($"Could not load sprite at path: {newWeapon.spritePath}");
        }

        weaponDataList.Add(newWeapon);
        currentCooldowns.Add(0f);  // Use "Add(0f)" to allow immediate fire OR use "Add(newWeapon.CoolDownDuration)"
        return true;
    }

    protected virtual void Attack(Weapon weaponData)
    {
        if (weaponData.loadedPrefab == null)
        {
            Debug.LogError($"Missing loadedPrefab for: {weaponData.weaponName}");
            return;
        }

        GameObject instance = Instantiate(weaponData.loadedPrefab, transform.position, Quaternion.identity);
        instance.transform.parent = transform;

        var knife = instance.GetComponent<KnifeBehaviour>();
        if (knife != null && pm != null)
        {
            knife.DirectionChecker(pm.lastMovedVector);
        }

        var projectile = instance.GetComponent<ProjectileWeaponBehaviour>();
        if (projectile != null)
        {
            projectile.Initialize(weaponData.speed, weaponData.damage, weaponData.pierce, weaponData.cooldownDuration);
        }
    }
}
