using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarlicBehaviour : MeleeWeaponBehaviour
{
    List<GameObject> markedEnemies;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        markedEnemies = new List<GameObject>();
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy") && !markedEnemies.Contains(col.gameObject))
        {
            IDamagable enemy = col.GetComponent<IDamagable>();
            enemy.TakeDamage(weaponData.Damage);
            markedEnemies.Add(col.gameObject); // Mark the enemy so it dosen't take another instance of damage from this garlic
        }
    }
}
