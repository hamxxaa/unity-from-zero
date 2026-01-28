# Neon Tank Defender

A top-down arcade shooter prototype focusing on neon aesthetics and endless wave survival mechanics.

## Overview
Players control a tank in a procedurally generated cyber-grid arena. The goal is to survive against increasing waves of enemies using physics-based movement and shooting mechanics. The project heavily utilizes Unity's Universal Render Pipeline (URP) for visual effects.

## Unity Fundamentals Used

- **URP Post-Processing**: Used Bloom, Vignette, and Tone Mapping for the neon glow aesthetic.
- **Material Emission**: High Dynamic Range (HDR) colors and emission maps for glowing objects.
- **Physics-Based Movement**: `Rigidbody` physics for tank movement and collision handling.
- **Raycasting**: Mouse position detection in 3D space for turret aiming (`ScreenPointToRay`).
- **Quaternion Rotation**: `Quaternion.LookRotation()` for aiming the turret towards the mouse cursor.
- **Singleton Pattern**: `GameManager` and `SoundManager` for global state management.
- **Coroutines**: `IEnumerator` for handling wave timings and enemy spawn intervals.
- **TextMeshPro (TMP)**: UI elements using dynamic glow shaders and pulse effects.
- **World Space UI**: Health bars that follow moving entities using `RectTransform`.
- **Events & Delegates**: Decoupling the Health system from the UI logic.
- **Instantiate & Destroy**: Dynamic object lifecycle management for bullets and enemies.