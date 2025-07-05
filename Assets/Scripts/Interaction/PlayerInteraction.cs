using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactionRange = 2f;
    [SerializeField] private LayerMask interactionLayer;
    [SerializeField] private Transform interactionOrigin; // Assign player's camera transform

    [Header("Events")]
    public UnityEvent<IInteractable> OnInteractableFound = new UnityEvent<IInteractable>();
    public UnityEvent OnNoInteractableFound = new UnityEvent();

    private IInteractable currentInteractable;
    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        
        if (interactionOrigin == null)
        {
            // Try to auto-assign the main camera if not set
            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                interactionOrigin = mainCam.transform;
            }
            else
            {
                Debug.LogError($"{nameof(PlayerInteraction)}: No interaction origin assigned and no main camera found!");
                enabled = false;
                return;
            }
        }
    }

    private void Update()
    {
        CheckForInteractables();
    }

    private void CheckForInteractables()
    {
        if (interactionOrigin == null) return;

        Ray ray = new Ray(interactionOrigin.position, interactionOrigin.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactionRange, interactionLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null && interactable.IsInteractable)
            {
                if (currentInteractable != interactable)
                {
                    currentInteractable = interactable;
                    OnInteractableFound?.Invoke(currentInteractable);
                }
                return;
            }
        }

        if (currentInteractable != null)
        {
            currentInteractable = null;
            OnNoInteractableFound?.Invoke();
        }
    }

    public void HandleInteraction()
{
    if (currentInteractable != null && currentInteractable.IsInteractable)
    {
        currentInteractable.OnInteract(interactionOrigin);
    }
}

    // Optional: For debugging in the editor
    private void OnDrawGizmosSelected()
    {
        if (interactionOrigin != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(interactionOrigin.position, interactionOrigin.forward * interactionRange);
        }
    }
}
