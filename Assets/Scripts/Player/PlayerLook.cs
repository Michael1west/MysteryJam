using UnityEngine;

[RequireComponent(typeof(PlayerState))]
public class PlayerLook : MonoBehaviour
{
    [Header("Look Settings")]
    public float mouseSensitivity = 0.2f;
    public float minPitch = -90f;
    public float maxPitch = 90f;
    public Transform cameraTransform;

    private float pitch = 0f;
    private PlayerState state;

    private void Awake()
    {
        state = GetComponent<PlayerState>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UpdateLook(Vector2 lookInput)
    {
        // Rotate player horizontally (yaw)
        float yaw = lookInput.x * mouseSensitivity;
        transform.Rotate(Vector3.up * yaw);

        // Rotate camera vertically (pitch)
        pitch -= lookInput.y * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        
        if (cameraTransform != null)
        {
            cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }
    }

    public void ToggleCursorLock()
    {
        bool isLocked = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = isLocked ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isLocked;
    }
}