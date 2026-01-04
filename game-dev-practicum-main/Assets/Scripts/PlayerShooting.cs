using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public float shootCooldown = 0.3f;
    public float bulletSpawnDistance = 0.5f; // Distance from player center to spawn bullet
    public float bulletSpawnHeight = 0.3f; // Height offset (positive = up, negative = down)

    private Animator animator;
    private float lastShootTime;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // This will be called automatically when Behavior is "Send Messages"
    void OnShoot(InputValue value)
    {
        if (value.isPressed)
        {
            Shooting();
        }
    }

    void Shooting()
    {
        if (Time.time < lastShootTime + shootCooldown)
        {
            Debug.Log("Cooldown active");
            return;
        }

        lastShootTime = Time.time;

        Debug.Log("SHOOTING!");

        // Check if bullet prefab is assigned
        if (bulletPrefab == null)
        {
            Debug.LogError("Bullet Prefab is not assigned! Please assign it in the Inspector.");
            return;
        }

        // Trigger animation
        animator.SetTrigger("Shoot");

        // Calculate bullet spawn position based on player's facing direction
        Vector3 spawnPosition = transform.position;
        Quaternion spawnRotation = Quaternion.identity;

        // Check which way player is facing based on rotation
        if (transform.rotation.eulerAngles.y == 0)
        {
            // Facing right
            spawnPosition += Vector3.right * bulletSpawnDistance;
            spawnPosition += Vector3.up * bulletSpawnHeight; // Add height offset
            spawnRotation = Quaternion.Euler(0, 0, 0);
            Debug.Log("Shooting RIGHT");
        }
        else
        {
            // Facing left
            spawnPosition += Vector3.left * bulletSpawnDistance;
            spawnPosition += Vector3.up * bulletSpawnHeight; // Add height offset
            spawnRotation = Quaternion.Euler(0, 180, 0);
            Debug.Log("Shooting LEFT");
        }

        Debug.Log("Spawning bullet at: " + spawnPosition);

        // Spawn bullet - the Bullet.cs script on the prefab handles everything else
        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, spawnRotation);

        if (bullet != null)
        {
            Debug.Log("Bullet spawned successfully!");
        }
        else
        {
            Debug.LogError("Failed to spawn bullet!");
        }
    }
}