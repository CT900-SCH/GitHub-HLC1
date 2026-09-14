using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : SimpleSingleton<Player>
{
    [Header("Player's Current Stats: ")]
    public string className;

    public float MaxHP = 10;
    public float HP;

    public float moveSpeed = 5f;
    public string description;

    public int ID;

    [Header("Player's Invincibility: ")]
    private bool isInvincible = false;
    public float invincibilityDuration = 1.5f; // seconds
    private float invincibilityTimer;

    private Rigidbody2D rb;
    [HideInInspector]
    public float lastHorizontalVector;
    [HideInInspector]
    public float lastVerticalVector;
    [HideInInspector]
    public Vector2 moveDir;
    [HideInInspector]
    public Vector2 lastMovedVector;

    ///This is for the animation - Joycelyn
    public Animator anim;

    /// This is for checking which class is selected  - Joycelyn
    UIPlayerSelection playerSelection;
    Player player;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastMovedVector = new Vector2(1, 0f);
        HP = MaxHP;
    }

    void Update()
    {
        Inputmanagement();
        PlayerInvincibility();
    }

    void FixedUpdate()
    {
        Move();
    }

    ///To select the correct class sprites - Jsoycelyn
    void ClassChecker()
    {
        if (ID == 0)
        {
            anim.SetBool("isBarb", true);
            anim.SetBool("isRog", false);
            anim.SetBool("isAlc", false);
        }
        if (ID == 1)
        {

            anim.SetBool("isRog", true);
            anim.SetBool("isBarb", false);
            anim.SetBool("isAlc", false);
        }
        if (ID == 2)
        {
            anim.SetBool("isAlc", true);
            anim.SetBool("isBarb", false);
            anim.SetBool("isRog", false);
        }
    }

    void Inputmanagement()
    {
        ///To select the correct class sprites - Jsoycelyn
        ClassChecker();

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDir = new Vector2(moveX, moveY).normalized;

        ///This is for the animation - Joycelyn
        anim.SetFloat("Walk", -1);

        if (moveDir.x != 0)
        {
            lastHorizontalVector = moveDir.x;
            lastMovedVector = new Vector2(lastHorizontalVector, 0f);

            ///This is for the animation - Joycelyn
            anim.SetFloat("Walk", Mathf.Abs(lastHorizontalVector));
            var player = gameObject.GetComponent<SpriteRenderer>();
            if (lastMovedVector.x < 0) 
            {
                player.flipX = true;
            }
            else
            {
                player.flipX = false;
            }
            //Debug.Log("hori val: " + lastHorizontalVector);
        }

        if (moveDir.y != 0)
        {
            lastVerticalVector = moveDir.y;
            lastMovedVector = new Vector2(0f, lastVerticalVector);

            ///This is for the animation - Joycelyn
            anim.SetFloat("Walk", Mathf.Abs(lastVerticalVector));
            Debug.Log("hori val: " + lastVerticalVector);

        }

        if (moveDir.x != 0 && moveDir.y != 0)
        {
            lastMovedVector = new Vector2(lastHorizontalVector, lastVerticalVector);
        }
    }

    void PlayerInvincibility()
    {
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0f)
            {
                isInvincible = false;
                Debug.Log("Player is no longer invincible.");
            }
        }
    }

    void Move()
    {
        rb.linearVelocity = new Vector2(moveDir.x * moveSpeed, moveDir.y * moveSpeed);
    }

    public void TakeDamage(int amount)
    {
        if (isInvincible) return;

        if (HP <= 0)
        {
            Debug.Log("You're dead");
        }
        
        HP -= amount;

        // Activate temporary invincibility
        isInvincible = true;
        invincibilityTimer = invincibilityDuration;
        Debug.Log("Player is now invincible.");
    }
}

