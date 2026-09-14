using UnityEngine;
using System.Collections.Generic;

public class RangedEnemyQuickFire : EnemyController
{
    private Transform playerTransform;
    private float fireCooldownTimer;

    public GameObject bulletPrefab;
    public Transform pointOFire;

    //Randomized Patrol settings around player
    public float patrolRadius = 10f;
    public int patrolPoints = 3;
    public float patrolPointTreshold = 0.1f;
    private float patrolOffset = 3f;

    private List<Vector2> generatedPatrolPoints; //List to store generated patrol points;
    private int currentPatrolPoint = 0;

    private Rigidbody2D rb;

    public float rotationSpeed = 1.0f;

    [Header("Enemy's Current Stats: ")]
    public float expGained = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        playerTransform = playerGO.transform;

        if (pointOFire == null)
        {
            pointOFire = transform;
        }

        fireCooldownTimer = 0f;
        generatedPatrolPoints = new List<Vector2>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        fireCooldownTimer = baseFireRate;
        GeneratePatrol();
    }
    // Update is called once per frame
    void Update()
    {
        //Face the player

        Vector2 lookDirection = (playerTransform.position - transform.position);
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler (0, 0, angle);
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        
        
        
        PatrolMovement();

        //Enemy will shoot forward as it goes
        fireCooldownTimer -= Time.deltaTime;
        if (fireCooldownTimer <= 0)
        {
            ShootBullet();
            if (baseFireRate > 0)
            {
                fireCooldownTimer = 1f / baseFireRate;
            }
            else
            {
                fireCooldownTimer = 3.0f; //default anchor if firerate is 0
            }
        }
    }

    private void GeneratePatrol()
    {
        generatedPatrolPoints.Clear();

        Vector2 playerPos = new Vector2 (playerTransform.position.x, playerTransform.position.y);

        for (int i = 0; i < patrolPoints; i++)
        {
            // Generate a random spawn position within a radius around the player. 
            Vector2 offsetDirection = (new Vector2 (transform.position.x, transform.position.y) - playerPos).normalized;
            Vector2 targetPatrolCenter = playerPos + offsetDirection * patrolOffset;

            Vector2 randomCircleOffset = Random.insideUnitCircle * patrolRadius;
            Vector2 randomPatrolPoint = targetPatrolCenter + randomCircleOffset;

            generatedPatrolPoints.Add(randomPatrolPoint);
        }
        currentPatrolPoint = 0;
    }

    private void PatrolMovement()
    {
        Vector2 targetPoint = generatedPatrolPoints[currentPatrolPoint];
        Vector2 currentPosition = rb.position;
        
        Vector3 directionToTarget = (targetPoint - currentPosition);

        //Moving
        rb.linearVelocity = directionToTarget * baseMovementSpeed;

        //Change Patrol Point 
        if (Vector2.Distance(currentPosition, targetPoint) < patrolPointTreshold)
        {
            currentPatrolPoint = (currentPatrolPoint +1 ) % generatedPatrolPoints.Count;
        }

        
    }
    
    private void ShootBullet()
    {
        Debug.Log("Attempting to shoot bullet!");
        GameObject newBullet = Instantiate (bulletPrefab, pointOFire.position, transform.rotation);
        Bullet bullet = newBullet.GetComponent<Bullet>();
        Vector2 bulletDirection = transform.right;
        bullet.InitializeBullet(baseDamageAmount, baseBulletSpeed, baseBulletSize, baseBulletTime, bulletDirection);
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
