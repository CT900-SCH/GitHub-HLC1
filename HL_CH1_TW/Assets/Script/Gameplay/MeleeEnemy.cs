using UnityEngine;
using UnityEngine.UIElements;

public class MeleeEnemy : EnemyController
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Enemy's Current Stats: ")]
    private Transform playerTransform;
    private float attackCooldownTimer;
    public float attackRange = 1f;
    public float timeBetweenAttacks;
    private Rigidbody2D rb;

    public GameObject attackRangeIndicator;
    public float indicatorDisplay = 0.2f;

    [Header("Enemy's Current Stats: ")]
    public float expGained = 2f;

    void Awake()
    {
        GameObject GO = GameObject.FindGameObjectWithTag("Player");
        playerTransform = GO.transform;
        attackCooldownTimer = 0f;
        rb = GetComponent<Rigidbody2D>();
    }

    public void Start()
    {
        if (baseWeaponSwingSpeed <= 0f)
        {
            timeBetweenAttacks = 1f/ baseWeaponSwingSpeed;
        }
        else
        {
            timeBetweenAttacks = 1f; //default swing speed if there are no attacks. 
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 lookDirection = (playerTransform.position - transform.position);
        Debug.Log(playerTransform.position);


        var enemyDir = gameObject.GetComponent<SpriteRenderer>();
        if (lookDirection.x < 0)
            enemyDir.flipX = true;
        else
            enemyDir.flipX = false;


        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f); // 10f is rotation speed.
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);


        //Attack
        attackCooldownTimer -= Time.deltaTime;
        if (distanceToPlayer <= attackRange)
        {
            if (attackCooldownTimer <= 0f)
            {
                PerformMeleeAttack();
                attackCooldownTimer = timeBetweenAttacks;
            }
            rb.linearVelocity = Vector2.zero;
            return;
        }

        //Chase
        rb.linearVelocity = lookDirection * baseMovementSpeed;
 
    }

    private void PerformMeleeAttack()
    {
        Debug.Log("PerformMelee running");
        Player.Instance.TakeDamage(baseDamageAmount);
        VisualizeAttackRange();
    }

    private void VisualizeAttackRange()
    {
        GameObject indicator = Instantiate (attackRangeIndicator, transform.position, Quaternion.identity);
        indicator.transform.SetParent(this.transform);
        // indicator.transform.localScale = Vector3.one * (attackRange * 2);
        Destroy (indicator, indicatorDisplay);
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
