using UnityEngine;

[RequireComponent(typeof(PlayerState))]
public class PlayerCrouch : MonoBehaviour
{
    [Header("Crouch Settings")]
    public float crouchSpeed = 2.5f;
    public float crouchHeight = 1f;       // Height when crouching
    public float standingHeight = 2f;     // Height when standing
    public float cameraStandingY = 0.6f;  // Camera Y position when standing
    public float cameraCrouchingY = 0.3f; // Camera Y position when crouching
    public float crouchSmoothTime = 0.1f;
    public LayerMask obstacleMask;

    private PlayerState state;
    private CapsuleCollider capsuleCollider;
    private Transform cameraTransform;
    private float heightVelocity;
    private float cameraYVelocity;
    private Vector3 standingCenter = Vector3.zero;
    private Vector3 targetCenter;
    private Vector3 centerVelocity;

    private void Awake()
    {
        state = GetComponent<PlayerState>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        cameraTransform = GetComponentInChildren<Camera>()?.transform;

        // Initialize heights and centers
        state.CurrentHeight = standingHeight;
        state.TargetHeight = standingHeight;
        targetCenter = standingCenter;
        
        // Set initial collider properties
        if (capsuleCollider != null)
        {
            capsuleCollider.height = state.CurrentHeight;
            capsuleCollider.center = standingCenter;
        }
    }

    private void Update()
    {
        UpdateCrouch();
    }

    public void ToggleCrouch()
    {
        if (state.IsCrouching)
        {
            // Only stand up if there's enough space above
            if (!IsCeilingBlocked())
            {
                state.IsCrouching = false;
                state.TargetHeight = standingHeight;
                targetCenter = standingCenter;
                state.IsSprinting = false;
            }
        }
        else
        {
            state.IsCrouching = true;
            state.TargetHeight = crouchHeight;
            targetCenter = new Vector3(0, (crouchHeight - standingHeight) * 0.5f, 0);
            state.IsSprinting = false;
        }
    }

    private void UpdateCrouch()
    {
        // Check if we hit our head while jumping
        if (!state.IsCrouching && IsCeilingBlocked())
        {
            state.IsCrouching = true;
            state.TargetHeight = crouchHeight;
            targetCenter = new Vector3(0, (crouchHeight - standingHeight) * 0.5f, 0);
        }

        // Smoothly adjust height and center
        if (capsuleCollider != null)
        {
            // Smoothly transition height
            state.CurrentHeight = Mathf.SmoothDamp(
                state.CurrentHeight,
                state.TargetHeight,
                ref heightVelocity,
                crouchSmoothTime
            );

            // Smoothly transition center
            Vector3 currentCenter = Vector3.SmoothDamp(
                capsuleCollider.center,
                targetCenter,
                ref centerVelocity,
                crouchSmoothTime
            );

            // Update collider
            capsuleCollider.height = state.CurrentHeight;
            capsuleCollider.center = currentCenter;

            // Update camera height
            if (cameraTransform != null)
            {
                float targetCameraY = state.IsCrouching ? cameraCrouchingY : cameraStandingY;
                Vector3 cameraPos = cameraTransform.localPosition;
                cameraPos.y = Mathf.SmoothDamp(cameraPos.y, targetCameraY, ref cameraYVelocity, crouchSmoothTime);
                cameraTransform.localPosition = cameraPos;
            }
        }
    }

    public bool IsCeilingBlocked()
    {
        if (capsuleCollider == null) return false;
        
        float rayLength = (standingHeight - crouchHeight) * 0.6f;
        float radius = capsuleCollider.radius * 0.9f;
        
        Vector3 rayStart = transform.position + Vector3.up * (capsuleCollider.height * 0.5f - radius);
        
        // Center ray
        if (Physics.Raycast(rayStart, Vector3.up, out _, rayLength, obstacleMask))
            return true;
            
        // Additional rays in a circle around the center
        int rayCount = 8;
        for (int i = 0; i < rayCount; i++)
        {
            float angle = i * (360f / rayCount);
            Vector3 dir = Quaternion.Euler(0, angle, 0) * transform.forward * radius * 0.7f;
            if (Physics.Raycast(rayStart + dir, Vector3.up, out _, rayLength, obstacleMask))
                return true;
        }
        
        return false;
    }
}
