using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerState))]
[RequireComponent(typeof(PlayerInputHandler))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerLook))]
[RequireComponent(typeof(PlayerJump))]
[RequireComponent(typeof(PlayerCrouch))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public Transform groundCheck;
    public CapsuleCollider capsuleCollider;

    private PlayerState state;
    private PlayerInputHandler inputHandler;
    private PlayerMovement movement;
    private PlayerLook look;
    private PlayerJump jump;
    private PlayerCrouch crouch;
    private PlayerInput playerInput;
    private Rigidbody rb;

    private void Awake()
    {
        // Get all required components
        state = GetComponent<PlayerState>();
        inputHandler = GetComponent<PlayerInputHandler>();
        movement = GetComponent<PlayerMovement>();
        look = GetComponent<PlayerLook>();
        jump = GetComponent<PlayerJump>();
        crouch = GetComponent<PlayerCrouch>();
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        
        // Initialize components
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        // Set up movement
        if (groundCheck != null)
        {
            movement.groundCheck = groundCheck;
        }

        // Set up look
        if (playerCamera != null)
        {
            look.cameraTransform = playerCamera.transform;
        }
        else if (Camera.main != null)
        {
            look.cameraTransform = Camera.main.transform;
        }

        // Set up rigidbody
        if (rb != null)
        {
            rb.freezeRotation = true;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }

        // Set up capsule collider if not assigned
        if (capsuleCollider == null)
        {
            capsuleCollider = GetComponent<CapsuleCollider>();
        }
    }

    private void OnValidate()
    {
        // Auto-assign camera if not set
        if (playerCamera == null && Camera.main != null)
        {
            playerCamera = Camera.main;
        }

        // Auto-find ground check if not set
        if (groundCheck == null)
        {
            var check = transform.Find("GroundCheck");
            if (check != null)
            {
                groundCheck = check;
            }
        }

        // Auto-get capsule collider if not set
        if (capsuleCollider == null)
        {
            capsuleCollider = GetComponent<CapsuleCollider>();
        }
    }

    public void ToggleCursorLock()
    {
        if (look != null)
        {
            look.ToggleCursorLock();
        }
    }
}
