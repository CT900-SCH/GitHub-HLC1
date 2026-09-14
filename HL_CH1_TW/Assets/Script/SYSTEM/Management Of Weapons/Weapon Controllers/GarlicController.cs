using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarlicController : WeaponController
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    //protected override void Attack()
    //{
    //    base.Attack();
    //    GameObject spawnedGarlic = Instantiate(weaponData.Prefab);
    //    spawnedGarlic.transform.position = transform.position;
    //    spawnedGarlic.transform.parent = transform;
    //}
}
