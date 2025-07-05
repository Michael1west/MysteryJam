using UnityEngine;

public interface IInteractable 
{
    string InteractionPrompt { get;}
    bool IsInteractable { get; }
    void OnInteract(Transform interactor);
}
