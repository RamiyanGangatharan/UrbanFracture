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

## Development Log

### Monday, May 12th, 2025
#### General
- Restarted the project due to critical breakage during character/controller port from another project.
- Implemented XML documentation for maintainability and future scaling.  
- Created terrain for the game.

#### UI Development
- Structured the UI namespace and split responsibilities into `MainMenuController` and `UIButtonAudio` components.
- Main menu now supports hover and click sound effects for button interactions.

#### First Person Character Control System
- Created a modular first person controller with:
    - Smooth acceleration-based movement using Unity's CharacterController.
    - Sprinting mechanics with FOV zoom via Cinemachine (sprinting causes a higher FOV compared to walking).
    - Double Jump functionality with a toggle and jump count reset on landing.
    - Gravity handling using a scaled physics approach for realistic falling.
    - Asynchronous velocity blending for smoother transitions.
    - Event-driven landing detection via UnityEvent.
- Implemented mouse-based camera functionality with:
    - Customizable pitch/yaw sensitivity.
    - Clampable vertical pitch for immersive control.
    - Rotation applied to both camera and player body.
- Added dynamic camera FOV shift when using Mathf.Lerp for a cinematic zoom effect.

#### Scene Management
- Created Asynchronous operations to manage the scenes and the loading of them
- Created a loading screen and functioning loading mechanics.
- Optimized loading to make it look smoother
- Made a GTA IV style load screen with an image slideshow

#### Audio
- Fixed audio issues with button mechanics.
- Modified the theme song to only feature the chorus and the ending portion of the original song
