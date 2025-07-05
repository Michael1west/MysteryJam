# Player Controller System Documentation

This document explains how the player controller system works in the MysteryJam game. The system is built using a component-based architecture, where each script handles a specific aspect of player control.

## Table of Contents
1. [PlayerController](#playercontroller)
2. [PlayerState](#playerstate)
3. [PlayerInputHandler](#playerinputhandler)
4. [PlayerMovement](#playermovement)
5. [PlayerLook](#playerlook)
6. [PlayerJump](#playerjump)
7. [PlayerCrouch](#playercrouch)

---

## PlayerController
**File:** `PlayerController.cs`

The main controller that coordinates all player-related components.

### Responsibilities:
- Manages references to all player components
- Initializes and sets up all required components
- Handles component dependencies

### Key Components:
- **Required Components:**
  - `PlayerState`
  - `PlayerInputHandler`
  - `PlayerMovement`
  - `PlayerLook`
  - `PlayerJump`
  - `PlayerCrouch`
  - `Rigidbody`
  - `CapsuleCollider`

### Important Methods:
- `Awake()`: Gets references to all required components
- `InitializeComponents()`: Sets up component references and initial states
- `OnValidate()`: Provides editor-time validation and auto-assignment

---

## PlayerState
**File:** `PlayerState.cs`

A data container that stores the current state of the player.

### State Properties:
- **Movement States:**
  - `IsGrounded`: Whether the player is on the ground
  - `IsCrouching`: Whether the player is crouching
  - `IsSprinting`: Whether the player is sprinting
  - `IsJumping`: Whether the player is jumping

- **Input Flags:**
  - `MoveInput`: 2D movement input (WASD/Left Stick)
  - `LookInput`: 2D look input (Mouse/Right Stick)
  - `JumpPressed`: Jump button state
  - `CrouchPressed`: Crouch button state
  - `SprintPressed`: Sprint button state

- **Physics Properties:**
  - `Velocity`: Current velocity
  - `CurrentHeight`: Current player height
  - `TargetHeight`: Target height for smooth transitions
  - `HeightVelocity`: Used for smooth height transitions
  - `CurrentSpeed`: Current movement speed

---

## PlayerInputHandler
**File:** `PlayerInputHandler.cs`

Handles all player input using Unity's Input System.

### Input Actions:
- `OnMove`: Handles movement input
- `OnLook`: Handles camera/look input
- `OnJump`: Handles jump input
- `OnSprint`: Handles sprint input
- `OnCrouch`: Handles crouch input

### Features:
- Converts raw input into game actions
- Updates the player state based on input
- Provides input validation and error handling

---

## PlayerMovement
**File:** `PlayerMovement.cs`

Handles the player's movement and ground detection.

### Features:
- Ground detection using sphere casting
- Movement in all directions
- Sprinting
- Air control
- Slope handling

### Key Methods:
- `Move()`: Applies movement based on input
- `SetVelocity()`: Directly sets player velocity
- `AddForce()`: Applies force to the player

### Settings:
- `moveSpeed`: Base movement speed
- `sprintMultiplier`: Speed multiplier when sprinting
- `airControlMultiplier`: Movement control in air
- `groundDistance`: Distance for ground detection

---

## PlayerLook
**File:** `PlayerLook.cs`

Handles camera rotation and looking around.

### Features:
- First-person camera control
- Vertical look limits
- Cursor locking

### Settings:
- `mouseSensitivity`: Look sensitivity
- `minPitch`: Minimum vertical look angle
- `maxPitch`: Maximum vertical look angle

### Methods:
- `UpdateLook()`: Updates camera rotation
- `ToggleCursorLock()`: Toggles mouse cursor lock

---

## PlayerJump
**File:** `PlayerJump.cs`

Manages jumping mechanics and air control.

### Features:
- Variable jump height
- Coyote time
- Fall acceleration
- Maximum fall speed

### Settings:
- `jumpForce`: Initial jump force
- `fallMultiplier`: Fall acceleration
- `lowJumpMultiplier`: Control for short hops
- `coyoteTime`: Time window after leaving ground when jump is still possible

### Methods:
- `TryJump()`: Attempts to make the player jump

---

## PlayerCrouch
**File:** `PlayerCrouch.cs`

Handles crouching and player height adjustments.

### Features:
- Smooth crouch/stand transitions
- Ceiling detection
- Camera height adjustment
- Collider resizing

### Settings:
- `crouchSpeed`: Movement speed while crouching
- `crouchHeight`: Height when crouching
- `standingHeight`: Height when standing
- `crouchSmoothTime`: Time for smooth transitions

### Methods:
- `ToggleCrouch()`: Toggles between crouch and stand
- `IsCeilingBlocked()`: Checks if there's room to stand up

---

## How It All Works Together

1. **Input Flow:**
   - `PlayerInputHandler` receives input and updates `PlayerState`
   - Input events trigger corresponding actions in other components

2. **Movement:**
   - `PlayerMovement` reads `MoveInput` from `PlayerState`
   - Applies movement based on ground state and input
   - Handles sprinting and air control

3. **Looking:**
   - `PlayerLook` processes `LookInput` from `PlayerState`
   - Rotates the camera and player independently

4. **Jumping:**
   - `PlayerJump` checks ground state and applies forces
   - Implements coyote time and variable jump height

5. **Crouching:**
   - `PlayerCrouch` manages height transitions
   - Handles ceiling detection
   - Adjusts camera and collider smoothly

This component-based architecture makes the system modular and easy to modify or extend.
