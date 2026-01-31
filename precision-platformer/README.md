# 2D Precision Platformer
A 2D platformer prototype with no level design and only focusing on core mechanics implementation, and Unity's 2D level design tools.

## Overview
Players navigate through obstacle-filled levels using tight, responsive controls. The objective is to collect all fruits in a level to unlock the exit door while avoiding traps like spikes. The project emphasizes fluid animation and modern 2D camera systems.

## Unity Fundamentals Used
Tilemap System: Grid-based level design using Unity's Tile Palette and Brushes.

2D Physics Optimization: Used CompositeCollider2D to merge complex tilemap colliders into a single geometry for performance.

Advanced Character Controller: Implemented "Coyote Time" and "Jump Buffering" mechanics for responsive, forgiving gameplay ("Game Feel").

Physics2D.OverlapBox: Precise ground detection logic using geometric overlap checks instead of simple raycasts.

Cinemachine 2D: Utilized Virtual Cameras with Framing Transposer, Dead Zones, Damping, and Lookahead for smooth player tracking.

Animator State Machine: Managing transitions between Idle, Run, Jump, and Fall states using parameters.

Sprite Editor: Slicing sprite sheets and manually adjusting pivot points for grounded animation accuracy.

Dynamic Level Progression: Logic to count specific tagged objects (Collectibles) in the scene to control level completion conditions.

Physics Materials: Used Friction settings to prevent wall-sticking behavior.
