# UrbanFracture Game Code - User Manual

## Overview

This project contains the core codebase for **UrbanFracture**, a first-person shooter game built with Unity. 
The code is organized into modular components handling player control, camera movement, combat mechanics, UI, and more.

---

## Table of Contents
- [Urban Fracture](#urban-fracture)
  - [Namespaces](#namespaces)
    - [Combat](#combat)
    - [Player](#player)
    - [UI](#ui)

> **Note:** This list is just for namespaces and will have more details inside each section.

## Urban Fracture
Urban Fracture is a first-person shooter (FPS) developed as an educational project to deepen the developer’s understanding of game development. Inspired by mid-2000s FPS classics such as Call of Duty, Garry’s Mod, and Escape from Tarkov, the game aims to capture the aesthetic and atmosphere of that era while delivering a blend of action, exploration, and tactical gameplay set in a dystopian, broken city environment. The project is designed with modularity in mind, enabling easy updates and maintenance. It serves as a practical learning platform for experimenting with game design, mechanics, and systems, rather than aiming to be a fully polished or commercial product. This undertaking followed the developer’s internship experience in Unity Engine development, acting as a self-directed continuation to refine skills and gain hands-on experience in building engaging gameplay.

## Namespaces
A namespace is a way to organize code logically and hierarchically. It groups related classes, structs, interfaces, and other types together to avoid naming conflicts and improve code readability and maintainability. Namespaces act like containers or folders that keep your code modular and prevent clashes between class names that might be similar but belong to different parts of your project or external libraries.

For example, in a project like UrbanFracture, you might have many classes named Gun, Player, or UIManager. Placing them in different namespaces (e.g., UrbanFracture.Combat, UrbanFracture.Player, UrbanFracture.UI) clarifies which part of the game each belongs to, allowing you to reuse class names in different contexts without conflict.

In my case, I use namespaces to help visually locate scripts quickly. For example, if I encounter a bug related to the camera, I check my Player namespace (e.g., UrbanFracture.Player.Components.LookHandler.cs) to find the responsible script. This is a personal preference, and you can use namespaces in whatever way best fits your project.


### Combat
... content ...

### Player
... content ...

### UI
... content ...

## Contact & Contributions

For questions or contributions, please reach out or submit pull requests via the project repository.

---

*This manual will be updated as the project evolves.*
