using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(PlayerState))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float sprintMultiplier = 1.5f;
    public float airControlMultiplier = 0.7f;
    public float groundDistance = 0.3f;
    public LayerMask groundMask;
    public Transform groundCheck;

    private Rigidbody rb;
    private PlayerState state;
    private Vector3 moveDirection;
    private bool wasGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        state = GetComponent<PlayerState>();
        rb.freezeRotation = true;
        
        if (groundCheck == null)
            Debug.LogError("GroundCheck reference is missing in PlayerMovement!");
    }

    private void FixedUpdate()
    {
        bool isGroundedNow = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        state.IsGrounded = isGroundedNow;
        
        if (wasGrounded != state.IsGrounded)
        {
            // Removed debug log
        }
        
        Move();
        wasGrounded = state.IsGrounded;
    }

    // In PlayerMovement.cs, update the Move() method:
private void Move()
{
    if (state.MoveInput.magnitude > 0.1f)
    {
        // Removed debug log
    }

    // Calculate direction
    moveDirection = (transform.right * state.MoveInput.x + transform.forward * state.MoveInput.y).normalized;

    // Apply movement speed with multipliers
    float currentSpeed = state.SprintPressed ? moveSpeed * sprintMultiplier : moveSpeed;
    // Apply air control if not grounded
    if (!state.IsGrounded)
    {
        currentSpeed *= airControlMultiplier;
        // Removed debug log
    }

    // Only modify horizontal velocity, preserve vertical velocity from physics
    Vector3 horizontalVelocity = moveDirection * currentSpeed;
    Vector3 currentVelocity = rb.linearVelocity;
    
    // Create new velocity with horizontal movement and preserved vertical movement
    Vector3 targetVelocity = new Vector3(horizontalVelocity.x, currentVelocity.y, horizontalVelocity.z);
    
    // Apply the velocity
    rb.linearVelocity = targetVelocity;
}

    // Visualize ground check in the editor
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            bool isGrounded = state != null ? state.IsGrounded : 
                Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
                
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
        }
    }

    public void SetVelocity(Vector3 velocity)
    {
        rb.linearVelocity = velocity;
    }

    public void AddForce(Vector3 force, ForceMode mode = ForceMode.Force)
    {
        rb.AddForce(force, mode);
    }
}
