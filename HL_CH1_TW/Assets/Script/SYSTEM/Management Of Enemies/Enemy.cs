using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootDrop
{
    public GameObject lootPrefab;
    [Range(0f, 100f)] public float dropChance; // in %
}


public class Enemy : MonoBehaviour, IDamagable
{
    Transform player;
    public float HP;
    public float moveSpeed;
    private bool isMoving = true;

    [Header("Loot Table Of Items: ")]
    public List<LootDrop> lootTable;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindObjectOfType<Player>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
            isMoving = true;
        else
            isMoving = false;

        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }

        EnemyDead();
    }

    public void TakeDamage(float damage)
    {
        HP -= damage;
    }

    public void EnemyDead()
    {
        if (HP <= 0)
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

            // Death
            GetComponent<Collider2D>().enabled = false;
            this.enabled = false;
            Destroy(gameObject, 1.5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            int damageValue = 2;

            if (player != null)
            {
                player.TakeDamage(damageValue); // or any amount of damage you want
            }
        }
    }
}
