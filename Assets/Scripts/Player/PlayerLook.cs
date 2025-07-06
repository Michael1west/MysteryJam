using UnityEngine;

[RequireComponent(typeof(PlayerState))]
public class PlayerLook : MonoBehaviour
{
    [Header("Look Settings")]
    [Tooltip("Mouse sensitivity (higher = more sensitive)")]
    [Range(0f, 1f)]
    public float mouseSensitivity = 0.3f;
    
    [Header("Smoothing")]
    [Tooltip("Smoothing factor (lower = more responsive, higher = smoother)")]
    [Range(0f, 0.3f)]
    public float smoothing = 0.03f;
    
    [Tooltip("The camera that will rotate up and down")]
    public Transform cameraTransform;

    // Rotation state
    private float xRotation = 0f;
    private float yRotation = 0f;
    private Vector2 currentLookVelocity;
    private Vector2 targetLookAngles;
    private Vector2 currentLookAngles;
    
    private PlayerState state;

    private void Awake()
    {
        state = GetComponent<PlayerState>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (cameraTransform == null)
        {
            cameraTransform = GetComponentInChildren<Camera>()?.transform;
            if (cameraTransform == null)
            {
                Debug.LogError("No camera found in children. Please assign a camera transform.");
            }
        }
        
        // Initialize rotations
        yRotation = transform.eulerAngles.y;
        xRotation = cameraTransform != null ? NormalizeAngle(cameraTransform.localEulerAngles.x) : 0f;
        targetLookAngles = new Vector2(xRotation, yRotation);
        currentLookAngles = targetLookAngles;
    }

    private void Update()
    {
        if (state == null) return;
        
        // Get look input from state (already processed by PlayerInputHandler)
        Vector2 lookInput = state.LookInput;
        
        if (lookInput.magnitude > 0.01f)
        {
            // Calculate target rotation using the mouseSensitivity variable
            targetLookAngles.x -= lookInput.y * mouseSensitivity;
            targetLookAngles.y += lookInput.x * mouseSensitivity;
            
            // Clamp vertical rotation
            targetLookAngles.x = Mathf.Clamp(targetLookAngles.x, -90f, 90f);
        }
        
        // Apply smoothing with velocity-based interpolation
        if (smoothing > 0.001f)
        {
            currentLookAngles = Vector2.SmoothDamp(
                currentLookAngles, 
                targetLookAngles, 
                ref currentLookVelocity, 
                smoothing,
                Mathf.Infinity,
                Time.unscaledDeltaTime
            );
        }
        else
        {
            currentLookAngles = targetLookAngles;
        }

        // Apply rotations
        if (cameraTransform != null)
        {
            cameraTransform.localRotation = Quaternion.Euler(currentLookAngles.x, 0f, 0f);
        }
        
        // Rotate player left/right (yaw)
        transform.rotation = Quaternion.Euler(0f, currentLookAngles.y, 0f);
    }
    
    private float NormalizeAngle(float angle)
    {
        // Normalize angle to -180 to 180 range
        while (angle > 180f) angle -= 360f;
        while (angle < -180f) angle += 360f;
        return angle;
    }

    public void ToggleCursorLock()
    {
        bool isLocked = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = isLocked ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = !isLocked;
    }
}