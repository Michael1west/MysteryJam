// PlayerInputHandler.cs
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
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

        if (state == null) Debug.LogError("PlayerState component is missing!");
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        state.MoveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        state.LookInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        state.JumpPressed = context.performed;
        if (context.performed && jump != null)
        {
            jump.TryJump();
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        state.SprintPressed = context.performed;
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        state.CrouchPressed = context.performed;
        if (context.performed && crouch != null)
        {
            crouch.ToggleCrouch();
        }
    }

    //public void OnInteract(InputAction.CallbackContext context)
    //{
        //if (context.performed && interaction != null)
        //{
         //   interaction.OnInteractPerformed(context);
        //}
   // }
}