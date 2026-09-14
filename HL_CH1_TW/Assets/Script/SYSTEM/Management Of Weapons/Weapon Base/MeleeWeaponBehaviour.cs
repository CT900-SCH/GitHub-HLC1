using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Base script for all Melee Weapon
/// </summary>
public class MeleeWeaponBehaviour : MonoBehaviour
{
    public WeaponScriptableObject weaponData;
    protected Vector3 direction;
    public float destroyAfterSeconds;

    //Current Stats
    protected float currentDamage;
    protected float currentSpeed;
    protected float currentCoolDownDuration;
    protected int currentPierce;

    void Awake()
    {
        currentDamage = weaponData.Damage;
        currentSpeed = weaponData.Speed;
        currentCoolDownDuration = weaponData.CoolDownDuration;
        currentPierce = weaponData.Pierce;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            IDamagable enemy = col.GetComponent<IDamagable>();
            enemy.TakeDamage(weaponData.Damage);
            ReducePierce();
        }
    }

    void ReducePierce()
    {
        currentPierce--;
        if (currentPierce <= 0)
        {
            Destroy(gameObject);
        }
    }
}
