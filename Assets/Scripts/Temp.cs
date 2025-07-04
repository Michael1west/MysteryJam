using UnityEngine;
using UnityEngine.InputSystem;

public class Temp : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;
    public float mouseSensitivity = 0.2f;
    public float jumpForce;
    public float airControlMultiplier = 0.7f;
    
    [Header("Sprinting")]
    public float sprintMultiplier = 1.5f;
    private bool isSprinting;

    [Header("Falling")]
    public float fallMultiplier = 2.5f;
    public float maxFallSpeed = 30f;

    [Header("Crouching")]
    public float crouchSpeed = 2.5f;
    public float crouchHeight = 0.5f;
    public float standingHeight = 1f;
    public float crouchSmoothTime = 0.2f;
    public float ceilingCheckDistance = 0.2f;
    private bool isCrouching;
    private float targetHeight;
    private float currentHeight;
    private float heightVelocity;
    private Vector3 standingCenter = new Vector3(0, 0, 0);
    private Vector3 crouchingCenter = new Vector3(0, -0.5f, 0);



    [Header("References")]
    public Transform cameraTransform;
    public LayerMask groundMask;
    public Transform groundCheck;
    public float groundDistance;

    private Rigidbody rb;
    private float pitch = 0f;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool isCursorLocked = true;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        UpdateCursorState();
        targetHeight = standingHeight;
        currentHeight = standingHeight;
    }

    void FixedUpdate()
    {
        //ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //Handle movement
        float currentSpeed = isCrouching ? crouchSpeed :
                        (isSprinting ? moveSpeed * sprintMultiplier : moveSpeed);
        
        Vector3 move = (transform.right * moveInput.x + transform.forward * moveInput.y).normalized * currentSpeed;
        
        //Apply movement
        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);

        //Air Control
        if (!isGrounded)
        {
            //reduce control in air
            move *= airControlMultiplier;

            //apply faster falling
            if (rb.linearVelocity.y < 0) //if is falling
            {
                rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;

                //clamp max fall speed
                if (rb.linearVelocity.y < -maxFallSpeed)
                {
                    rb.linearVelocity = new Vector3(rb.linearVelocity.x, -maxFallSpeed, rb.linearVelocity.z);
                }
            }
        }

        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);

        // Handle ceiling check - prevent standing if something is above
        if (!isCrouching && IsCeilingBlocked())
        {
            isCrouching = true;
            targetHeight = crouchHeight;
        }

        if (Mathf.Abs(currentHeight - targetHeight) > 0.01f)
        {
            currentHeight = Mathf.SmoothDamp(currentHeight, targetHeight, ref heightVelocity, crouchSmoothTime);
            Vector3 newScale = transform.localScale;
            newScale.y = currentHeight;
            transform.localScale = newScale;

            if (!Physics.Raycast(transform.position, Vector3.up, standingHeight / 2f))
            {
                transform.position = new Vector3(
                    transform.position.x,
                    transform.position.y + (newScale.y - transform.localScale.y) * 0.5f,
                    transform.position.z
                );
            }
        }
    }
    
    void Update()
    {
        //Handle mouse look
        Vector2 look = lookInput * mouseSensitivity;

        //Rotate player to the left/right
        transform.Rotate(Vector3.up * look.x);

        //Rotate camera up/down
        pitch -= look.y;
        pitch = Mathf.Clamp(pitch, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);

        //Toggle cursor lock with escape key
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            isCursorLocked = !isCursorLocked;
            UpdateCursorState();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    
    }

    public void OnJump(InputAction.CallbackContext context)
    {
       
        if (context.performed && isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        isSprinting = context.performed || context.started;
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        // Only respond to the performed phase to avoid double-toggling
        if (context.performed)
        {
            isCrouching = !isCrouching;
            targetHeight = isCrouching ? crouchHeight : standingHeight;
            Debug.Log($"Crouch toggled. New state: {isCrouching}");
        }
    }

    private void UpdateCursorState()
    {
        Cursor.lockState = isCursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !isCursorLocked;
    }

    private bool IsCeilingBlocked()
    {
        float rayLength = (standingHeight - crouchHeight) * 0.5f + 0.1f; // Add small buffer
        Vector3 rayOrigin = transform.position + Vector3.up * (crouchHeight * 0.5f);
        
        // Draw debug ray (visible in Scene view)
        Debug.DrawRay(rayOrigin, Vector3.up * rayLength, Color.red, 0.1f);
        
        // Check multiple points for better detection
        float radius = 0.3f; // Adjust based on your character's width
        bool hit = Physics.Raycast(rayOrigin, Vector3.up, rayLength, groundMask) ||
                  Physics.Raycast(rayOrigin + transform.right * radius, Vector3.up, rayLength, groundMask) ||
                  Physics.Raycast(rayOrigin - transform.right * radius, Vector3.up, rayLength, groundMask) ||
                  Physics.Raycast(rayOrigin + transform.forward * radius, Vector3.up, rayLength, groundMask) ||
                  Physics.Raycast(rayOrigin - transform.forward * radius, Vector3.up, rayLength, groundMask);
        
        return hit;
    }
}
