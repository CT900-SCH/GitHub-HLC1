using UnityEngine;

public class RangedEnemySlowFire : EnemyController
{
    private Transform playerTransform;
    private float fireCooldown;
    private Rigidbody2D rb;

    public GameObject bulletPrefab;
    public Transform firePoint;
    public float rotationSpeed = 5f;

    [Header("Enemy's Current Stats: ")]
    public float expGained = 2f;

    void Awake()
    {
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        playerTransform = playerGO.transform;
        if (firePoint == null)
        {
            firePoint = transform;
        }

        fireCooldown = 0f;
        rb = GetComponent<Rigidbody2D>();

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fireCooldown = 1f / baseFireRate;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 lookDirection = (playerTransform.position - transform.position);
        float angle = Mathf.Atan2 (lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0)
        {
            ShootBullet();
            fireCooldown = 1f/ baseFireRate;
        }
    }

    private void ShootBullet()
    {
        GameObject newBulletGO = Instantiate (bulletPrefab, firePoint.position, transform.rotation);
        Bullet bullet = newBulletGO.GetComponent<Bullet>();
        Vector2 bulletDirection = transform.right;
        bullet.InitializeBullet(baseDamageAmount, baseBulletSpeed, baseBulletSize, baseBulletTime, bulletDirection);
    }

    void OnCollissionEnter2D(Collision2D collision)
    {
        rb.linearVelocity = Vector2.zero;
    }

    void OnColissionStay2D (Collision2D collision)
    {
        rb.linearVelocity += Vector2.zero;
    }

    protected override void Die()
    {
        if (baseHealth <= 0)
        {
            // Give Exp
            PlayerLevelUpStats.Instance.SetExperience(expGained);
            Debug.LogWarning($"{expGained} experience gained from killing {gameObject.name}.");

            // Count Kills
            PlayerLevelUpStats.Instance.Kills += 1;

            // Drop Loot
            foreach (var loot in lootTable)
            {
                float roll = Random.Range(0f, 100f);
                if (roll <= loot.dropChance && loot.lootPrefab != null)
                {
                    Instantiate(loot.lootPrefab, transform.position, Quaternion.identity);
                    break; // Drop only one item; remove this if multiple drops allowed
                }
            }

            // Stop movement completely
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.isKinematic = true;
            }

            // Death
            GetComponent<Collider2D>().enabled = false;
            this.enabled = false;
            Destroy(gameObject, 1.5f);
        }
    }
}
