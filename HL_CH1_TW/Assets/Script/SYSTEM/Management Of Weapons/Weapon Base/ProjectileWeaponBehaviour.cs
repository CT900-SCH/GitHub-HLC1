using System.Security.Cryptography;
using UnityEngine;

public class ProjectileWeaponBehaviour : MonoBehaviour
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

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    public void Initialize(float speed, int damage, int pierce, float cooldown)
    {
        currentSpeed = speed;
        currentDamage = damage;
        currentPierce = pierce;
        currentCoolDownDuration = cooldown;
    }

    public void DirectionChecker(Vector3 dir)
    {
        direction = dir;

        float dirX = direction.x;
        float dirY = direction.y;

        Vector3 scale = transform.localScale;
        Vector3 rotation = transform.rotation.eulerAngles;

        if(dirX < 0 && dirY == 0) // Left
        {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
        }
        else if(dirX == 0 && dirY < 0) // Down
        {
            scale.y = scale.y * -1;
        }
        else if (dirX == 0 && dirY > 0) // Up
        {
            scale.x = scale.x * -1;
        }
        else if (dirX > 0 && dirY > 0) // Right Up
        {
            rotation.z = 0f;
        }
        else if (dirX > 0 && dirY < 0) // Right Down
        {
            rotation.z = -90f;
        }
        else if (dirX < 0 && dirY > 0) // Left Up
        {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
            rotation.z = -90f;
        }
        else if (dirX < 0 && dirY < 0)
        {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
            rotation.z = 0f;
        }

        transform.localScale = scale;
        transform.rotation = Quaternion.Euler(rotation);
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
        if(currentPierce <= 0)
        {
            Destroy(gameObject);
        }
    }
}
