using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerState))]
public class PlayerInputHandler : MonoBehaviour
{
    private PlayerState state;
    private PlayerMovement movement;
    private PlayerLook look;
    private PlayerJump jump;
    private PlayerCrouch crouch;

    private void Awake()
    {
        state = GetComponent<PlayerState>();
        movement = GetComponent<PlayerMovement>();
        look = GetComponent<PlayerLook>();
        jump = GetComponent<PlayerJump>();
        crouch = GetComponent<PlayerCrouch>();
        
        // Verify all required components
        if (movement == null) Debug.LogError("PlayerMovement component missing!");
        if (look == null) Debug.LogError("PlayerLook component missing!");
        if (jump == null) Debug.LogError("PlayerJump component missing!");
        if (crouch == null) Debug.LogError("PlayerCrouch component missing!");
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        state.MoveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        state.LookInput = context.ReadValue<Vector2>();
        if (look != null)
        {
            look.UpdateLook(state.LookInput);
        }
        else
        {
            Debug.LogError("PlayerLook component is null!");
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        state.JumpPressed = context.performed || context.canceled;
        
        if (context.performed)
        {
            if (jump != null)
            {
                jump.TryJump();
            }
            else
            {
                Debug.LogError("PlayerJump component is null!");
            }
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        state.SprintPressed = context.performed || context.canceled;
        state.IsSprinting = state.SprintPressed && !state.IsCrouching;
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        state.CrouchPressed = context.performed || context.canceled;
        
        if (context.performed)
        {
            if (crouch != null)
            {
                crouch.ToggleCrouch();
            }
            else
            {
                Debug.LogError("PlayerCrouch component is null!");
            }
        }
    }
}
