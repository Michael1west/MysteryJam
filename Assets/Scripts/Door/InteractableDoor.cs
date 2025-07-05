using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class InteractableDoor : Door, IInteractable
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactionDistance = 2f;
    [SerializeField] private string lockedMessage = "The door is locked.";
    [SerializeField] private string interactionPrompt = "Open Door";
    [SerializeField] private LayerMask playerLayer;
    
    private Vector3 closedRotation;
    private Vector3 openRotation;
    private Transform playerTransform;

    private void Start()
    {
        closedRotation = transform.rotation.eulerAngles;
        openRotation = new Vector3(closedRotation.x, closedRotation.y + openAngle, closedRotation.z);
        
        // Find the player
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    public override void Interact()
    {
        if (isMoving) return;
        
        if (isOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    public override void Open()
    {
        if (isOpen || isLocked) return;
        
        isMoving = true;
        StartCoroutine(RotateDoor(openRotation, () => {
            isOpen = true;
            isMoving = false;
            OnDoorOpened?.Invoke();
        }));
    }

    public override void Close()
    {
        if (!isOpen || isLocked) return;
        
        isMoving = true;
        StartCoroutine(RotateDoor(closedRotation, () => {
            isOpen = false;
            isMoving = false;
            OnDoorClosed?.Invoke();
        }));
    }

    public override void Toggle()
    {
        if (isMoving) return;
        
        if (isOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    private System.Collections.IEnumerator RotateDoor(Vector3 targetRotation, System.Action onComplete = null)
    {
        Quaternion startRot = transform.rotation;
        Quaternion endRot = Quaternion.Euler(targetRotation);
        float time = 0f;
        
        while (time < 1f)
        {
            time += Time.deltaTime * openSpeed;
            transform.rotation = Quaternion.Slerp(startRot, endRot, time);
            yield return null;
        }
        
        onComplete?.Invoke();
    }

    // Optional: Check if player is looking at the door and in range
    public bool CanBeInteracted()
    {
        if (playerTransform == null) return false;
        
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        if (distance > interactionDistance) return false;
        
        Vector3 directionToDoor = (transform.position - playerTransform.position).normalized;
        float dotProduct = Vector3.Dot(playerTransform.forward, directionToDoor);
        
        return dotProduct > 0.5f; // Player is facing the door
    }

    public string InteractionPrompt => isLocked ? lockedMessage : interactionPrompt;
    public bool IsInteractable => !isMoving;

    public void OnInteract(Transform interactor)
    {
        if (isLocked)
        {
            Debug.Log(lockedMessage);
            return;
        }

        Interact();
    }
}