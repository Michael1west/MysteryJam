using UnityEngine;

public class DoorInteractable : Interactable
{
    [Header("Door Settings")]
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float openSpeed = 2f;
    [SerializeField] private Transform doorPivot;
    [SerializeField] private bool isLocked = false;
    [SerializeField] private string lockedMessage = "The door is locked.";
    
    private bool isOpen = false;
    private Vector3 closedRotation;
    private Vector3 openRotation;
    private float rotationProgress = 0f;
    private bool isRotating = false;

    private void Start()
    {
        if (doorPivot == null)
        {
            doorPivot = transform;
        }
        
        // Store initial rotation as closed position
        closedRotation = doorPivot.localEulerAngles;
        // Calculate open position
        openRotation = new Vector3(
            closedRotation.x,
            closedRotation.y + openAngle,
            closedRotation.z
        );
    }

    public override void Interact(PlayerState playerState)
    {
        if (isLocked)
        {
            Debug.Log(lockedMessage);
            return;
        }

        if (isRotating) return; // Prevent multiple interactions during rotation

        isOpen = !isOpen;
        isRotating = true;
        rotationProgress = 0f;
        
        Debug.Log($"Door {(isOpen ? "opening" : "closing")} to: {(isOpen ? openRotation : closedRotation)}");
    }

    private void Update()
{
    if (!isRotating) return;

    // Update rotation progress
    rotationProgress += Time.deltaTime * openSpeed;
    
    // Calculate current rotation
    float currentAngle = Mathf.Lerp(
        isOpen ? 0 : openAngle,
        isOpen ? openAngle : 0,
        Mathf.Clamp01(rotationProgress)
    );
    
    // Apply rotation around the Y axis
    doorPivot.localEulerAngles = new Vector3(
        closedRotation.x,
        closedRotation.y + currentAngle,
        closedRotation.z
    );

    // Check if rotation is complete
    if (rotationProgress >= 1f)
    {
        isRotating = false;
        Debug.Log($"Door rotation complete. Final rotation: {doorPivot.localEulerAngles}");
    }
}

    public override void OnFocus()
    {
        interactionPrompt = isLocked ? lockedMessage : 
            (isOpen ? "Close [E]" : "Open [E]");
    }

    public override void OnUnfocus()
    {
        // Optional: Reset prompt or do nothing
    }
}