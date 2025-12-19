using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 3f; // Destroy after 3 seconds

    void Start()
    {
        Debug.Log("Bullet Start() called - Bullet is alive!");
        Debug.Log("Bullet position: " + transform.position);
        Debug.Log("Bullet rotation: " + transform.rotation.eulerAngles);

        // Get Rigidbody2D component
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            Debug.Log("Rigidbody2D found!");
            // Move bullet based on its rotation (set by PlayerShooting)
            // Right if rotation is 0°, Left if rotation is 180°
            if (Mathf.Approximately(transform.rotation.eulerAngles.y, 0f))
            {
                rb.linearVelocity = Vector2.right * speed;
                Debug.Log("Moving RIGHT with velocity: " + rb.linearVelocity);
            }
            else if (Mathf.Approximately(transform.rotation.eulerAngles.y, 180f))
            {
                rb.linearVelocity = Vector2.left * speed;
                Debug.Log("Moving LEFT with velocity: " + rb.linearVelocity);
            }
            else
            {
                Debug.LogWarning("Bullet rotation is neither 0 nor 180! Rotation Y: " + transform.rotation.eulerAngles.y);
            }
        }
        else
        {
            Debug.LogError("Bullet is missing Rigidbody2D component!");
        }

        // Check if sprite renderer exists and has a sprite
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            if (sr.sprite != null)
            {
                Debug.Log("Sprite Renderer has sprite: " + sr.sprite.name);
            }
            else
            {
                Debug.LogError("Sprite Renderer has NO SPRITE assigned!");
            }
        }
        else
        {
            Debug.LogError("Bullet has NO Sprite Renderer!");
        }

        // Auto-destroy bullet after lifetime
        Destroy(gameObject, lifetime);
        Debug.Log("Bullet will be destroyed in " + lifetime + " seconds");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Bullet hit: " + other.gameObject.name + " with tag: " + other.tag);

        // Ignore collision with player (check both tag and name)
        if (other.CompareTag("Player") || other.gameObject.name == "Player")
        {
            Debug.Log("Hit player - ignoring");
            return;
        }

        // Ignore collision with other bullets
        if (other.GetComponent<Bullet>() != null)
        {
            Debug.Log("Hit another bullet - ignoring");
            return;
        }

        // Destroy bullet when hitting anything else (enemies, walls, ground, etc.)
        Debug.Log("Destroying bullet!");
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        Debug.Log("Bullet destroyed!");
    }
}