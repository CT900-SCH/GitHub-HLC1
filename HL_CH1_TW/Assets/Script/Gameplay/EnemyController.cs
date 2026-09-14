using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyController : MonoBehaviour, IDamagable
{
   // create variables that store base and modified values from JSON file.
    
    public string enemyName;
    public float baseHealth;
    public int maxHealth;
    public int baseDamageAmount; 
    public float baseMovementSpeed; 
    public float baseFireRate;
    public float baseWeaponSwingSpeed;
    public float baseBulletSize;
    public float baseBulletSpeed;
    public float baseBulletTime;

    protected string assignedPattern;
    private JSONReader.EnemyClass storedEnemyData;

    [Header("Loot Table Of Items: ")]
    public List<LootDrop> lootTable;

    public void Initialize(JSONReader.EnemyClass enemyData, int gameLevel) //create variable enemyData and brings in data fron JSON reader.
    {
        storedEnemyData = enemyData;
        
        // Store base stats from JSON
        enemyName = enemyData.name;
        assignedPattern = enemyData.enemyPattern;

        ApplyLevelScaling(gameLevel, storedEnemyData.health, storedEnemyData.damageDealt, storedEnemyData.movementSpeed, storedEnemyData.firerate, storedEnemyData.weaponSwingSpeed, storedEnemyData.bulletSize, storedEnemyData.bulletSpeed, storedEnemyData.bulletTime);
        baseHealth = maxHealth;

        Debug.Log($"Initialized {enemyName} for Level {gameLevel}: Health={baseHealth}, Damage={baseDamageAmount}, Speed={baseMovementSpeed}");
    }

    private void ApplyLevelScaling(int gameLevel, int presetHealth, int presetDamage, int presetMovespeed, float presetFirerate, int presetSwingSpeed, float presetBulletSize, float presetBulletSpeed, float presetBulletTime) 
    {
        float healthMultiplier = 1f + (gameLevel - 1) * 0.10f; // 10% health increase per level 
        float damageMultiplier = 1f + (gameLevel - 1) * 0.15f; //15% increase per level
        float speedMultiplier = 1f + (gameLevel - 1) * 0.10f; //10% increase per level 
        float bulletSpeedMultiplier = 1f + (gameLevel - 1) * 0.05f; //5% increase per level

        //no bullet size, fireRate, SwingSpeed changes between levels. 

        maxHealth = (int)(presetHealth * healthMultiplier);
        baseDamageAmount = (int)(presetDamage * damageMultiplier);
        baseMovementSpeed = (presetMovespeed * speedMultiplier);
        baseFireRate = presetFirerate;
        baseWeaponSwingSpeed = presetSwingSpeed;
        baseBulletSize = presetBulletSize;
        baseBulletSpeed = (presetBulletSpeed * bulletSpeedMultiplier);
        baseBulletTime = presetBulletTime;



        //Ensure enemies do not have too high stats

        baseHealth = Mathf.Max(1, maxHealth);
        baseDamageAmount = Mathf.Max(1, baseDamageAmount);
        baseMovementSpeed = Mathf.Max(0.1f, baseMovementSpeed);
        baseFireRate = Mathf.Max(0.1f, baseFireRate);
        baseWeaponSwingSpeed = Mathf.Max(2f, baseWeaponSwingSpeed);
        baseBulletSpeed = Mathf.Max(1f, baseBulletSpeed);
        baseBulletTime = Mathf.Max(10f, baseBulletTime);
    }

    public void TakeDamage (float playerDamage)
    {
        baseHealth -= playerDamage;
        if (baseHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        if (baseHealth <= 0)
        {
            // Give Exp
            float expValueWhenDead = 2f;
            PlayerLevelUpStats.Instance.SetExperience(expValueWhenDead);

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

    public virtual void SetEnemyPattern()
    {
        // does nothing to allow enemies to override
    }
}
