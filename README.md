# Urban Fracture

**Urban Fracture** is a first-person shooter (FPS) developed as an educational project. It aims to capture the aesthetic 
and gameplay feel of mid-2000s FPS titles, drawing inspiration from classics such as *Call of Duty*, *Garry's Mod*, 
and *Escape from Tarkov*. Set in a dystopian, fractured urban environment, the game blends action, exploration, 
and tactical combat. This project is not intended for commercial release. Instead, it serves as a platform for experimenting 
with game design principles, mechanics, and systems. The primary goal is to practice and refine game development skills 
while recreating the tone and atmosphere of iconic shooters from the era.

> **Important:** Please refer to my [USER MANUAL](/Assets/Scripts/UserManual.md) regarding the code for more information.

---

> [!WARNING]
> **Before cloning this project, install Git LFS (Large File Storage)** to ensure all assets are properly downloaded.

# Getting Started

## Prerequisites
- **Unity 6000.x or newer** (for game development)
- **Visual Studio** or any compatible C# IDE (for scripting)

## Installation

1. Clone the repository:
   ```bash
    git clone -b main https://github.com/RamiyanGangatharan/UrbanFracture.git UrbanFracture
   ```
2. Open the project in Unity 6000.x
3. Press play in the `MainMenu` Scene

## Controls

| Action               | Key/Mouse Input     |
|----------------------|---------------------|
| Move                 | `WASD`              |
| Look Around          | `Mouse`             |
| Fire Weapon          | `Left Click`        |
| Aim Down Sights      | `Right Click`       |
| Reload               | `R`                 |
| Sprint               | `Left Shift`        |
| Interact             | `E`                 |
| Pause / Open Menu    | `Escape`            |


## Primary Features
- Smooth first-person movement
- Gunplay with multiple weapons, each featuring unique handling and characteristics
- Health and damage system
- AI enemy behaviors
- UI elements:
    - Main menu
    - Loading screens
    - In-game HUD
    - Pause menu

## Audio
- Sound Effects
    - Weapon fire
    - Footsteps
    - Ambient environmental audio
- Music
    - Main Menu soundtrack
    - Optional Background music during gameplay

## Development 
- Engine: Unity 6000.1.2f1
- Language: C#
- Tools Used:
    - Unity Engine (Game Development)
    - Visual Studio 2022 (Code Editing)
    - FL Studio (Audio Production)

---

## Resources Used

This project was built using a combination of custom systems and learning resources from the game development community. Key references include:

- [How to Add and Manage Background Music in Unity](https://damiandabrowski.medium.com/how-to-add-and-manage-background-music-in-your-unity-projects-86cd5889a542)
- [UI Slider Tutorial (YouTube)](https://youtu.be/oya8_SlLXb0)
- [Loading Screen Implementation (YouTube)](https://youtu.be/NyFYNsC3H8k)
- [First-Person Movement Tutorial (YouTube)](https://youtu.be/41MD0s9FiXI)
- [Weapon Systems Tutorial (YouTube)](https://youtu.be/JCngTlb2R2c)
- [Muzzle Flash Particle System (YouTube)](https://youtu.be/rf7gHVixmmc)
- Custom sand impact particle system created with the help of ChatGPT

---
## Development Log

---

### Saturday, May 17th, 2025
- Added weapon holstering mechanics
- Removed recoil mechanics (I will be re-implementing it later)
---

### Friday, May 16th, 2025

- Looked at textures and am redesigning the map.

---

### Thursday, May 15th, 2025

#### General
- Converted the repository to use Git LFS (Large File Storage).
- Overhauled the README for clarity and structure.
- Rebuilding the map to focus on a single large building instead of a full city layout.

---

### Wednesday, May 14th, 2025

#### General
- Implemented a functional in-game HUD displaying player health, ammo count, and weapon information.
- Integrated initial weapon particle effects, including muzzle flash and placeholder impact visuals.
- Started debugging the recoil system — logic implemented, but no visible impact yet.

#### First-Person Character System
- Developed a modular player health system to support damage and healing.
- Added weapon SFX (shooting, reloading) using `AudioSource` components.
- Implemented a simple crosshair UI for aiming feedback.
- Improved weapon interaction by connecting audio, UI, and input logic.

#### Unity Particle System
- Replaced Unity’s default particle pack with custom-made VFX for visual consistency.
- Designed and implemented a muzzle flash system tailored for FPS weapons.
- Created a sand impact particle system to simulate bullet hits on terrain using custom parameters.

---

### Tuesday, May 13th, 2025

#### General
- Refactored the player controller into multiple files for better organization.
  - Adopted a hub-and-spoke architecture for modular expansion.
- Imported player models and terrain assets.
- Created an initial terrain for testing.

#### First-Person Character System
- Added a player model (`Survivalist Character`), but removed it due to setup complexity.
- Implemented camera bobbing for movement immersion.

#### Weapon System

**General Weapon Features**
- Integrated pistol model.
- Implemented firing, reloading, and (in-progress) recoil mechanics.
- Added a UI crosshair via Unity Canvas.

**Codebase Structure**
- Created modular classes:
  - `Gun`
  - `Pistol`
  - `Recoil`
  - `GunData` (Scriptable Object)

#### Audio
- Implemented footstep sounds.

---

### Monday, May 12th, 2025

#### General
- Restarted the project after a critical failure from importing old controller scripts.
- Added XML documentation to the codebase for maintainability.
- Created basic terrain for testing and prototyping.

---

#### UI Development
- Refactored UI structure:
  - Introduced `MainMenuController` and `UIButtonAudio` to separate visual and audio logic.
- Enhanced main menu:
  - Added responsive hover/click audio feedback.
- Fixed inconsistent audio playback in UI elements.

---

#### First-Person Character System
- Built a modular **First Person Controller** with:
  - Smooth, acceleration-based movement via `CharacterController`.
  - Sprinting with Cinemachine-driven dynamic FOV changes.
  - Configurable double-jump with automatic reset on landing.
  - Enhanced airborne physics with custom gravity scaling.
  - Asynchronous velocity blending for fluid movement.
  - Landing detection via Unity Events for extensibility.

- Implemented advanced **Mouse Look System**:
  - Adjustable pitch/yaw sensitivity.
  - Natural head movement limits with pitch clamping.
  - Real-time rotation syncing between player and camera.

- Integrated cinematic FOV transitions using `Mathf.Lerp` for immersive sprint effects.

---

#### Player Input Integration
- Connected Unity’s Input System via a centralized `Player` component.
- Mapped key actions using `InputValue`:
  - Move: `WASD`
  - Look: `Mouse`
  - Sprint: `Left Shift`
  - Jump: `Spacebar`

---

#### Scene Management and Loading
- Created asynchronous scene loading to prevent freezing.
- Designed a loading screen with:
  - Real-time progress bar.
  - GTA IV-style slideshow featuring in-game imagery.
- Optimized load times for smoother transitions.

---

#### Audio Enhancements
- Resolved persistent audio playback issues in UI.
- Edited the main theme:
  - Trimmed to emphasize the chorus and ending for a memorable loop.

---
