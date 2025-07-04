using UnityEngine;

[System.Serializable]
public class PlayerState : MonoBehaviour
{
    // Movement states
    public bool IsGrounded { get; set; }
    public bool IsCrouching { get; set; }
    public bool IsSprinting { get; set; }
    public bool IsJumping { get; set; }

    // Input flags
    public Vector2 MoveInput { get; set; }
    public Vector2 LookInput { get; set; }
    public bool JumpPressed { get; set; }
    public bool CrouchPressed { get; set; }
    public bool SprintPressed { get; set; }

    // Physics properties
    public Vector3 Velocity { get; set; }
    public float CurrentHeight { get; set; }
    public float TargetHeight { get; set; }
    public float HeightVelocity { get; set; }

    // Movement parameters
    public float CurrentSpeed { get; set; }

    private void Awake()
    {
        // Initialize default values
        IsGrounded = false;
        IsCrouching = false;
        IsSprinting = false;
        IsJumping = false;
        MoveInput = Vector2.zero;
        LookInput = Vector2.zero;
        Velocity = Vector3.zero;
        CurrentHeight = 2f;
        TargetHeight = 2f;
        HeightVelocity = 0f;
        CurrentSpeed = 0f;
    }
}
