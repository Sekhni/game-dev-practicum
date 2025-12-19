using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Jump Settings")]
    public float jumpForce = 10f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveInput;
    private bool isGrounded;

    public GrenadeManager cm;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb.freezeRotation = true;
    }

    // This will be called automatically when Behavior is "Send Messages"
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // Jump input
    void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded)
        {
            Jump();
        }
    }

    void Update()
    {
        // Check if player is on the ground
        CheckGrounded();

        float speed01 = Mathf.Clamp01(Mathf.Abs(rb.linearVelocity.x) / Mathf.Max(0.0001f, moveSpeed));
        animator.SetFloat("Speed", speed01, 0.1f, Time.deltaTime);

        if (moveInput.x > 0.01f)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (moveInput.x < -0.01f)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
    }

    void CheckGrounded()
    {
        // Check if there's a ground check object
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }
        else
        {
            // Fallback: check slightly below the player
            Vector2 checkPosition = new Vector2(transform.position.x, transform.position.y - 0.5f);
            isGrounded = Physics2D.OverlapCircle(checkPosition, groundCheckRadius, groundLayer);
        }
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        Debug.Log("JUMP!");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("grenade"))
        {
            cm.grenadeCount++;
            Destroy(other.gameObject);
        }
    }

    // Draw ground check radius in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (groundCheck != null)
        {
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
        else
        {
            Vector2 checkPosition = new Vector2(transform.position.x, transform.position.y - 0.5f);
            Gizmos.DrawWireSphere(checkPosition, groundCheckRadius);
        }
    }
}