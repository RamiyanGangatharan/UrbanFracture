# URBAN FRACTURE

Urban Fracture is a first-person shooter (FPS) developed for educational purposes. The game is designed to evoke the look and feel of mid-2000s FPS titles, drawing inspiration from iconic games like Call of Duty, Garry's Mod, and Escape from Tarkov. It's a project intended to help the developer gain experience in game design, mechanics, and systems, offering a mix of action, exploration, and tactical gameplay within a dystopian, broken city environment.

This project is created purely for educational purposes to practice and refine game development skills. The goal is not to produce a fully polished or commercial game, but rather to experiment with game mechanics and systems while capturing the aesthetic and vibe of classic FPS games.

--- 

# Getting Started

## Prerequisites:

- Unity 6000.x+ for development
- Visual Studio or another equivalent C# IDE for scripting

## Installing
- Clone this repository `git clone -b main https://github.com/RamiyanGangatharan/UrbanFracture.git UrbanFracture`

- Open the project in Unity 6000.x
- Press play in the MainMenu Scene

## Controls
- `WASD` for keyboard player movement
- `MOUSE` to move your camera around your character
- `LEFT_CLICK` for Firing your weapon
- `RIGHT_CLICK` to aim down sights
- `R` to reload weapon
- `LEFT_SHIFT` to sprint
- `E` to interact with objects
- `ESCAPE` to use the pause menu

## Primary Features
- Smooth First-Person movement 
- Gunplay: Multiple Weapons with unique handling and attributes
- Health & Damage functionality
- AI Enemies
- UI Elements such as a main menu, loading screens, in-game HUD's and a pause menu.

## Audio
- Sound Effects
    - Weapon sounds
    - Footsteps
    - Ambient Audio
- Music
    - Main Menu soundtrack
    - Optional Background music during gameplay

## Development 
- Unity Version 6000.1.2f1
- Language: C#
- Tools Used:
    - Unity Engine [Game Engine]
    - Visual Studio [IDE for coding]
    - FL Studio [For audio creation]

---

## Resources Used:
- [Background Music Implementation](https://damiandabrowski.medium.com/how-to-add-and-manage-background-music-in-your-unity-projects-86cd5889a542)
- [UI Slider](https://youtu.be/oya8_SlLXb0)
- [Loading Screen](https://youtu.be/NyFYNsC3H8k)
- [First-Person Movement](https://youtu.be/41MD0s9FiXI)
- [Weapon Systems](https://youtu.be/JCngTlb2R2c)
---

## Development Log

### Wednesday, May 14th, 2025

---

### General:
- Implemented a functional in-game HUD (health, ammo, weapon info).
- Integrated basic particle effects for weapons (e.g. muzzle flash & impact placeholders).
- Began debugging recoil system — logic added, but no visible impact yet.
- Muzzle flash effects currently not rendering; under investigation.

### First Person Character System:
- Introduced a modular health system for the player.
- Added weapon SFX (shooting, reloading).
- Added a crosshair for the weapon
- Implemented a simple crosshair UI element for aiming feedback.

### Tuesday, May 13th, 2025

---

### General
- Split up the player controller into multiple files to keep it more organized.
    - Organized it into a hub and spoke, controller type of system, will implement that concept into later systems.
- Imported Playermodels
- Imported Terrain assets
- Created a terrain

### First Person Character System
- Implemented a player model called `Survivalist character`.
- Added then removed the player model as it is too complicated to configure at the moment
- added camera bobbing

### Weapon System

- General Weapon System Implementation
    - Implemented a Pistol Model
    - Implemented firing mechanics
    - Implemented reloading mechanics
    - Implemented recoil mechanics [currently broken]
    - Created a crosshair on a canvas

- General Codebase for the Weapon System
    - Created a Gun Class
    - Created a Pistol Class
    - Created a Recoil Class
    - Created a GunData Scriptable Object

### Audio
- Added footstep sounds

---

### Monday, May 12th, 2025

---

### General
- Restarted the project from scratch after a critical failure caused by attempting to port a character and controller script from a previous project.
- Implemented XML documentation throughout the codebase for improved maintainability and future scalability.
- Designed and added basic terrain for initial gameplay testing and level prototyping.

---

### UI Development
- Refactored UI structure:
  - Introduced `MainMenuController` and `UIButtonAudio` components to separate visual logic from audio interactions.
- Enhanced main menu functionality:
  - Added responsive hover and click sound effects for interactive feedback.
- Fixed issues with inconsistent audio playback in UI elements.

---

### First-Person Character System
- Developed a modular **First Person Controller** featuring:
  - Smooth acceleration-based movement using `CharacterController`.
  - Sprinting with Cinemachine-driven dynamic FOV changes for immersive speed feedback.
  - Configurable double-jump system with toggle and automatic reset upon landing.
  - Scaled gravity application for more realistic airborne movement and fall behavior.
  - Asynchronous velocity blending for seamless movement transitions.
  - Landing detection with Unity Events for potential animation or effect triggers.
  
- Implemented advanced **mouse look system**:
  - Adjustable pitch/yaw sensitivity.
  - Pitch clamping for natural head movement limits.
  - Real-time camera and player rotation for a fluid FPS experience.
  
- Integrated cinematic FOV transitions using `Mathf.Lerp` to visually enhance sprinting.

---

### Player Input Integration
- Connected Unity's new Input System to gameplay through a `Player` component.
  - Mapped the following actions using `InputValue`:
    - Movement: `WASD`
    - Look: `Mouse`
    - Sprint: `Left Shift`
    - Jump: `Spacebar`
- Centralized input logic for better maintainability and flexibility across gameplay features.

---

### Scene Management and Loading
- Created asynchronous scene loading logic to support non-blocking transitions.
- Designed and implemented a loading screen with:
  - Progress feedback to improve UX during scene changes.
  - A GTA IV-style slideshow showcasing in-game images for visual engagement.
- Optimized load performance to create a smoother, more polished transition experience.

---

### Audio Enhancements
- Fixed persistent audio playback issues in the UI interaction system.
- Edited the game’s theme song:
  - Trimmed to highlight the chorus and closing sections for a more memorable intro/loop.

---

