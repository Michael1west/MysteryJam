using UnityEngine;

public class InteractionManager : MonoBehaviour
{
   [Header("References")]
   [SerializeField] private PlayerState playerState;
   [SerializeField] private Camera playerCamera;
   [SerializeField] private LayerMask interactionLayers;

   [Header("Settings")] 
   [SerializeField] private float interactionRange = 3f;
   [SerializeField] private float interactionRadius = 0.5f;

   // Current state
   private Interactable currentInteractable;
   private bool wasInteractPressed;

   private void Awake()
   {
    if (playerCamera == null)
    {
        playerCamera = Camera.main;
        if (playerCamera == null)
        {
            Debug.LogError("No camera assigned");
            enabled = false;
        }
    }
   }

   private void Update()
   {
        if (playerState == null || playerCamera == null) return;

        HandleInteractionDetection();
        HandleInteractionInput();
   }

   private void HandleInteractionDetection()
   {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        bool hitSomething = Physics.SphereCast(
            ray, 
            interactionRadius,
            out RaycastHit hit,
            interactionRange,
            interactionLayers
        );

        if (hitSomething)
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();

            if (interactable != currentInteractable)
            {
                // Unfocus previous interactable
                if (currentInteractable != null)
                {
                    currentInteractable.OnUnfocus();
                }

                // Set new interactable
                currentInteractable = interactable;

                // Focus on new interactable
                if (currentInteractable != null)
                {
                    currentInteractable.OnFocus();
                }
            }

            else if (currentInteractable != null)
            {
                // No hit, unfocus current interactable
                currentInteractable.OnUnfocus();
                currentInteractable = null;
            }
        }
   }

   private void HandleInteractionInput()
    {
        bool isInteractPressed = playerState.InteractPressed;
    
        if (isInteractPressed && !wasInteractPressed && currentInteractable != null)
        {
            currentInteractable.Interact(playerState);
        }
    
        wasInteractPressed = isInteractPressed;
    }
}
