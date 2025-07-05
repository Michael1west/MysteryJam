using UnityEngine;
using UnityEngine.Events;

public abstract class Door : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] protected float openAngle = 90f;
    [SerializeField] protected float openSpeed = 2f;
    [SerializeField] protected bool isLocked = false;
    
    [Header("Events")]
    public UnityEvent OnDoorOpened;
    public UnityEvent OnDoorClosed;
    public UnityEvent OnDoorLocked;
    public UnityEvent OnDoorUnlocked;

    protected bool isOpen = false;
    protected bool isMoving = false;

    // Abstract methods that derived classes must implement
    public abstract void Interact();
    public abstract void Open();
    public abstract void Close();
    public abstract void Toggle();

    // Common methods for all doors
    public virtual void Lock()
    {
        if (isLocked) return;
        isLocked = true;
        OnDoorLocked?.Invoke();
    }

    public virtual void Unlock()
    {
        if (!isLocked) return;
        isLocked = false;
        OnDoorUnlocked?.Invoke();
    }

    public bool IsOpen => isOpen;
    public bool IsLocked => isLocked;
    public bool IsMoving => isMoving;
}
