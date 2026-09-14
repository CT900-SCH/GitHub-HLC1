using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeBehaviour : ProjectileWeaponBehaviour
{
    List<GameObject> markedEnemies;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        //base.Update();
        transform.position += direction * weaponData.Speed * Time.deltaTime;
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Enemy"))
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
