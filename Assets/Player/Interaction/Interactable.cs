using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] protected string interactionPrompt = "Interact";
    [SerializeField] private float interactionRadius = 1.5f;

    // Getters
    public string InteractionPrompt => interactionPrompt;
    public float InteractionRadius => interactionRadius;

    // Called when the player looks at the interactable
    public virtual void OnFocus() { }

    // Called when the player looks away from interactable
    public virtual void OnUnfocus() { }

    // Called when the player interacts with the object
    public abstract void Interact(PlayerState playerState);
}
