# Tower Defense Game

A simple tower defense game prototype built with Unity contains only key game mechanics.

## Overview
This is a basic tower defense game where players place turrets to defend a base against incoming enemy waves. Turrets automatically target and shoot enemies within range.

## Unity Fundamentals Used

- **Prefab System**: Used prefabs for instantiation
- **Singleton Pattern**: GameManager uses singleton for global access
- **Raycasting**: Mouse click detection for turret placement
- **Vector3 Operations**: Distance calculations, direction vectors, position interpolation
- **Quaternion Rotation**: `Quaternion.Lerp()` for smooth turret rotation, `LookRotation()` for targeting
- **New Input System**: Mouse and Keyboard input via `UnityEngine.InputSystem`
- **Vector3.MoveTowards()**: Simple linear movement toward target
- Target-following behavior
- Distance-based collision detection
- **Fire Rate System**: Time-based shooting cooldown using `Time.deltaTime`
- **Range Detection**: Sphere-based range checking
- Reusable Health component for enemies and base
- Damage/heal functionality
- Health bar UI integration
- **Coroutines**: `IEnumerator` and `yield return` for timed enemy spawning
- Continuous wave spawning with intervals
- **State Machine**: Enum-based game states (Start, Playing, GameOver, Pause)
- Scene management and UI panel toggling
