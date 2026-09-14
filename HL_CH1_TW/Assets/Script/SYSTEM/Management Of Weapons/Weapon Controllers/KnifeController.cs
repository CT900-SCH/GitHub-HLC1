using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeController : WeaponController
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    //protected override void Attack()
    //{
    //    foreach (var weaponData in weaponDataList)
    //    {
    //        GameObject spawnedWeapon = Instantiate(weaponData.Prefab);
    //        spawnedWeapon.transform.position = transform.position;
    //        spawnedWeapon.transform.parent = transform;

    //        // Apply direction if it’s a projectile weapon
    //        var knife = spawnedWeapon.GetComponent<KnifeBehaviour>();
    //        if (knife != null)
    //        {
    //            knife.DirectionChecker(pm.lastMovedVector);
    //            continue; // Already handled
    //        }

    //        // No need to do anything for garlic — it auto-attaches & marks enemies in Start
    //    }
    //}
}
