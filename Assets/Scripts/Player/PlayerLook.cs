using UnityEngine;

[RequireComponent(typeof(PlayerState))]
public class PlayerLook : MonoBehaviour
{
    [Header("Look Settings")]
    public float mouseSensitivity = 2f;
    public float minPitch = -90f;
    public float maxPitch = 90f;
    public Transform cameraTransform;
    [Tooltip("Smoothing time for camera movement (lower = more responsive)")]
    public float smoothTime = 0.1f;

    private float pitch = 0f;
    private float yaw = 0f;
    private float xVelocity = 0f;
    private float yVelocity = 0f;
    private PlayerState state;

    private void Awake()
    {
        state = GetComponent<PlayerState>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        if (cameraTransform == null)
        {
            Debug.LogError("Camera Transform is not assigned in PlayerLook!");
            cameraTransform = Camera.main?.transform;
        }
    }

    public void UpdateLook(Vector2 lookInput)
    {
        if (lookInput.magnitude > 0.01f)
        {
            // Apply sensitivity with deltaTime for frame-rate independence
            float deltaTime = Time.deltaTime;
            float targetYaw = yaw + (lookInput.x * mouseSensitivity * deltaTime * 60f); // 60f to maintain similar sensitivity to before
            float targetPitch = Mathf.Clamp(pitch - (lookInput.y * mouseSensitivity * deltaTime * 60f), minPitch, maxPitch);
        
            // Smooth the rotation
            yaw = Mathf.SmoothDamp(yaw, targetYaw, ref xVelocity, smoothTime);
            pitch = Mathf.SmoothDamp(pitch, targetPitch, ref yVelocity, smoothTime);
        
            // Apply rotations
            transform.rotation = Quaternion.Euler(0f, yaw, 0f);
        
            if (cameraTransform != null)
            {
                cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
            }
        }
    }

    public void ToggleCursorLock()
    {
        bool isLocked = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = isLocked ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isLocked;
    }
}