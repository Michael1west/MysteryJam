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
   [SerializeField] private float focusCooldown = 0.1f; // Cooldown between focus changes

   // Current state
   private Interactable currentInteractable;
   private Interactable previousInteractable;
   private bool wasInteractPressed;
   private float lastFocusChangeTime;
   private bool isChecking = false;

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
       if (playerState == null || playerCamera == null || isChecking) return;
       
       StartCoroutine(CheckForInteractions());
       HandleInteractionInput();
   }

   private System.Collections.IEnumerator CheckForInteractions()
   {
       isChecking = true;
       
       // Wait for cooldown if needed
       if (Time.time - lastFocusChangeTime < focusCooldown)
       {
           isChecking = false;
           yield break;
       }

       Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
       bool hitSomething = Physics.SphereCast(
           ray, 
           interactionRadius,
           out RaycastHit hit,
           interactionRange,
           interactionLayers,
           QueryTriggerInteraction.Collide
       );

       if (hitSomething)
       {
           Interactable interactable = hit.collider.GetComponent<Interactable>();
           
           // Only process if we have a valid interactable and it's different from current
           if (interactable != null && interactable != currentInteractable)
           {
               // Unfocus previous interactable if it exists
               if (currentInteractable != null)
               {
                   currentInteractable.OnUnfocus();
                   previousInteractable = currentInteractable;
               }

               // Set and focus on the new interactable
               currentInteractable = interactable;
               currentInteractable.OnFocus();
               lastFocusChangeTime = Time.time;
           }
       }
       else if (currentInteractable != null)
       {
           // No hit, but we have a current interactable - unfocus it
           currentInteractable.OnUnfocus();
           previousInteractable = currentInteractable;
           currentInteractable = null;
           lastFocusChangeTime = Time.time;
       }
       
       isChecking = false;
   }

   private void HandleInteractionInput()
   {
       if (playerState == null) return;
       
       bool isInteractPressed = playerState.InteractPressed;
   
       if (isInteractPressed && !wasInteractPressed && currentInteractable != null)
       {
           currentInteractable.Interact(playerState);
       }
   
       wasInteractPressed = isInteractPressed;
   }
   
   private void OnDisable()
   {
       // Clean up when disabled
       if (currentInteractable != null)
       {
           currentInteractable.OnUnfocus();
           currentInteractable = null;
       }
   }
}
