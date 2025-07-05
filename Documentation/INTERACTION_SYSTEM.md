# Interaction System Documentation

## Overview
The Interaction System provides a flexible way to handle player interactions with objects in the game world. It's built on a component-based architecture that separates input handling from interaction logic.

## Core Components

### 1. IInteractable Interface
All interactable objects must implement this interface.

**Properties:**
- `string InteractionPrompt`: The text to display when the player looks at the object
- `bool IsInteractable`: Whether the object can currently be interacted with

**Methods:**
- `void OnInteract(Transform interactor)`: Called when the player interacts with the object

### 2. PlayerInteraction
Handles detection and management of interactable objects.

**Inspector Settings:**
- `Interaction Range`: Maximum distance for interaction
- `Interaction Layer`: Layer mask for interactable objects
- `Interaction Origin`: Transform from which the interaction ray is cast (usually the camera)

**Events:**
- `OnInteractableFound`: Triggered when a new interactable is detected
- `OnNoInteractableFound`: Triggered when no interactable is in range

### 3. PlayerInputHandler
Handles input events and delegates them to the appropriate systems.

## Implementation Guide

### Making an Object Interactable
1. Create a script that implements `IInteractable`
2. Add the script to your GameObject
3. Optionally, add a collider if one isn't already present

### Example: Basic Interactable
```csharp
using UnityEngine;

public class ExampleInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string prompt = "Interact";
    
    public string InteractionPrompt => prompt;
    public bool IsInteractable => true;

    public void OnInteract(Transform interactor)
    {
        Debug.Log($"Interacted with {gameObject.name}");
        // Add interaction logic here
    }
}
```

### InteractableDoor
A ready-to-use door implementation that inherits from the base Door class and implements IInteractable.

**Features:**
- Lock/unlock functionality
- Customizable interaction prompts
- Face-checking to ensure player is facing the door

**Usage:**
1. Add the `InteractableDoor` component to a door GameObject
2. Configure the following in the Inspector:
   - `Interaction Distance`: How close the player needs to be
   - `Locked Message`: Message to show when door is locked
   - `Interaction Prompt`: Text to show when the door can be interacted with

## Best Practices
1. Keep interaction logic in the `OnInteract` method
2. Use the `IsInteractable` property to control when an object can be interacted with
3. Use layers to optimize raycasting performance
4. Provide clear feedback when interaction is not possible

## Troubleshooting

### Common Issues
1. **Interactions not working**
   - Ensure the object's layer is included in the Interaction Layer Mask
   - Verify the object has a collider
   - Check that `IsInteractable` returns true

2. **Interaction prompt not showing**
   - Ensure the UI is subscribed to the `OnInteractableFound` event
   - Check that `InteractionPrompt` returns a non-empty string

## Extending the System
To add new interaction types:
1. Create a new script that implements `IInteractable`
2. Add any necessary serialized fields for configuration
3. Implement the required interface members
4. Add the component to your GameObject
