using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 3f;
    public int damage = 10;

    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            if (Mathf.Approximately(transform.rotation.eulerAngles.y, 0f))
            {
                rb.linearVelocity = Vector2.right * speed;
            }
            else if (Mathf.Approximately(transform.rotation.eulerAngles.y, 180f))
            {
                rb.linearVelocity = Vector2.left * speed;
            }
        }

        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.gameObject.name == "Player")
        {
            return;
        }

        if (other.GetComponent<Bullet>() != null)
        {
            return;
        }

        // Changed from Health to Enemy
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}