using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 30;
    private int currentHealth;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public bool moveLeft = true;

    [Header("Combat")]
    public int attackDamage = 10;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.5f;
    public LayerMask playerLayer;
    public Transform attackPoint;

    [Header("Detection")]
    public Transform groundCheck;
    public Transform wallCheck;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isDead = false;
    private bool isAttacking = false;
    private float lastAttackTime = 0f;
    private Vector3 originalScale;
    private bool playerInRange = false;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalScale = transform.localScale;

        if (animator != null)
        {
            animator.SetBool("isWalking", true);
        }
    }

    void Update()
    {
        if (!isDead)
        {
            CheckForPlayer();

            if (!playerInRange && !isAttacking)
            {
                Move();
                CheckForObstacles();
            }
            else if (playerInRange)
            {
                // Stop movement when player is in range
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

                // Stop walking animation
                if (animator != null)
                {
                    animator.SetBool("isWalking", false);
                }
            }
        }
    }

    void Move()
    {
        // Resume walking animation when moving
        if (animator != null)
        {
            animator.SetBool("isWalking", true);
        }

        if (moveLeft)
        {
            rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
        else
        {
            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
    }

    void CheckForObstacles()
    {
        bool hasGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        bool hasWall = Physics2D.OverlapCircle(wallCheck.position, checkRadius, groundLayer);

        if (!hasGround || hasWall)
        {
            Flip();
        }
    }

    void CheckForPlayer()
    {
        if (attackPoint == null) return;

        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        if (hitPlayers.Length > 0)
        {
            playerInRange = true;

            // Face the player
            FacePlayer(hitPlayers[0].transform);

            // Attack if cooldown is ready
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                Attack(hitPlayers[0].gameObject);
            }
        }
        else
        {
            playerInRange = false;
        }
    }

    void FacePlayer(Transform player)
    {
        // Determine if player is to the left or right
        if (player.position.x < transform.position.x)
        {
            // Player is to the left
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
        else
        {
            // Player is to the right
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
    }

    void Attack(GameObject player)
    {
        isAttacking = true;
        lastAttackTime = Time.time;

        // Trigger attack animation
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        // Deal damage to player
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }

        StartCoroutine(ResumeAfterAttack(0.5f));
    }

    IEnumerator ResumeAfterAttack(float delay)
    {
        yield return new WaitForSeconds(delay);
        isAttacking = false;
    }

    void Flip()
    {
        moveLeft = !moveLeft;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        // Disable the Rigidbody2D component entirely
        if (rb != null)
        {
            rb.simulated = false;
        }

        if (animator != null)
        {
            animator.SetBool("isWalking", false);
            animator.SetTrigger("Die");
        }

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }

        Destroy(gameObject, 2f);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }

        if (wallCheck != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(wallCheck.position, checkRadius);
        }

        if (attackPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}