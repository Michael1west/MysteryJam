using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(PlayerState))]
public class PlayerJump : MonoBehaviour
{
    [Header("Jump Settings")]
    public float jumpForce = 5f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float maxFallSpeed = 20f;
    public float coyoteTime = 0.2f;
    
    private float coyoteTimeCounter;
    private Rigidbody rb;
    private PlayerState state;
    private PlayerMovement movement;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        state = GetComponent<PlayerState>();
        movement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        // Handle coyote time
        if (state.IsGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Better jumping
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            if (rb.linearVelocity.y < -maxFallSpeed)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, -maxFallSpeed, rb.linearVelocity.z);
            }
        }
        else if (rb.linearVelocity.y > 0 && !state.JumpPressed)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    public void TryJump()
    {
        if (coyoteTimeCounter > 0f)
        {
            // Reset vertical velocity before jumping for consistent jump height
            Vector3 velocity = rb.linearVelocity;
            velocity.y = 0f;
            rb.linearVelocity = velocity;
            
            // Apply jump force
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            
            // Reset coyote time
            coyoteTimeCounter = 0f;
            
            // Update state
            state.IsJumping = true;
        }
    }
}
