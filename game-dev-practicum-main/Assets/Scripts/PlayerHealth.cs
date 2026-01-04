using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    private int currentHealth;

    private Animator animator;
    private PlayerMovement playerMovement;
    private bool isDead = false;
    private GameOverManager gameOverManager;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        gameOverManager = FindObjectOfType<GameOverManager>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return; // Don't take damage if already dead

        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth); // Prevent negative health

        Debug.Log("Player took " + damage + " damage! Current health: " + currentHealth);

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return; // Prevent multiple death calls

        isDead = true;
        Debug.Log("Player died!");

        // Trigger death animation
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // Disable player movement
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        // Optional: Disable the player's Rigidbody2D
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false; // Stops all physics
        }

        // Optional: Disable collider so enemies stop attacking
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }

        // Show game over screen
        if (gameOverManager != null)
        {
            gameOverManager.ShowGameOver();
        }
    }

    // Optional: Method to heal the player
    public void Heal(int amount)
    {
        if (isDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth); // Don't exceed max health
        Debug.Log("Player healed " + amount + " health! Current health: " + currentHealth);
    }
}