# P2-UISystem-NataliyaLaptyk

## Student Info
| Field | Value |
|-------|-------|
| Name | Nataliya Laptyk |
| Student ID |2530192|

## Game Context
A 2.5D platformer based on the fairy tale "The Little Matchgirl" by Hans Christian Andersen. The player controls a little girl surviving through a dark winter level by collecting and lighting matches that provide warmth and light. Heat drains constantly at a configurable rate — lighting matches slows the drain but cannot stop it completely. Platforms are hidden in darkness and only revealed when a match is lit. The player dies when heat reaches zero, and wins by reaching the fireplace at the end of the level.
Moodboard:https://drive.google.com/drive/u/0/folders/1s1qHTbatZEmNW2G9yJyqGJ24XcYCbuzu

This game was originally made as a platformer for Salim's class.
Krys Palys - modeled all assets.
Alex Alexandre Nobre - did level greybox and texturing.
Juan Esteban Cruz Tovar modeled the main character and found the theme music.
I came up with the original idea for game mechanics and their implementation in code. The code that I did for Salim's class was different, didn't have a game manager. 
That is why, for this assignment, I redid all the code from scratch to make sure it meets all the requirements.
In my opinion, raycasting is not the best option for detecting objects in this game; it's much easier when an object is picked up on collision, because time is of the essence in this game. However, I wanted to implement what we had studied in our Scripting 2 class even if those design decisions were not the optimal ones for this particular game.   

## System Type
**Collectable Inventory** — the player picks up matches scattered across the level using a raycast interaction system (press E when facing a match). Matches are stacked by type in a slot-based inventory (Normal, Bright, Fragile, Special). Slots are created dynamically using prefab instantiation — no slots exist until matches are picked up.

## Main Menu Canvas Approach
Using Canvas Render Mode Screen Space – Overlay - best for UI that should always be visible and not affected by the camera's perspective. 
It renders UI elements on top of everything else in the scene, making it ideal for main menus, HUDs, and other interface elements that need to be consistently visible regardless of the camera's position or orientation.

## Event Approach

**HUD (Heat Bar, Burn Timer, Match Counter, Heat Text):** C# events via `GameManager` — chosen because heat drains every frame (high-frequency updates) and the updates are purely code-driven with no need for Inspector configuration. C# events are faster than UnityEvents for per-frame updates.

**Inventory:** `UnityEvent` on `PlayerInventory.OnInventoryChanged` — chosen because the game is small, performance is not a concern, and UnityEvent offers easier setup for a solo developer without sacrificing functionality.

## Controls
| Action | Keyboard |
|--------|----------|
| Move | WASD / Arrow Keys |
| Jump | Space |
| Collect match | E (while facing match) |
| Select inventory slot | 1, 2, 3, 4 |
| Light selected match | F |

## Features Implemented
The game includes a full title screen with Start, Settings (sound toggle), and Quit buttons. The in-game HUD displays a heat slider, real-time heat countdown text, match counter, and burn timer. The inventory system supports up to 4 unique match types with stacking — picking up a second Normal Match shows "x2" on the slot rather than creating a duplicate. When a match is lit, a burn progress bar drains on the active slot using the match's icon as the fill image. Platforms are hidden until a match is burning, then revealed via the `PlatformVisibilityByMatch` system. A death screen appears when heat reaches zero with a restart button, and a level complete zone loads the end scene.

## ScriptableObjects
Four `MatchData` assets are defined, one per match type: Normal Match, Bright Match, Fragile Match, and Special Match. Each defines burn duration, light range, light intensity, warmth per second, heat buffer gain, and FX references (audio, particles). Fields use `[SerializeField] private` with public read-only methods following the  `ItemData` pattern we saw in class.

## Known Issues / Limitations
The sound toggle in Settings pauses all audio via `AudioListener.pause` — a more granular audio mixer approach would be better for a shipped game. Camera follow is a custom script rather than Cinemachine. The burn progress bar only appears on the currently selected/burning slot.

## Project Structure
```
Assets/
├── Scripts/
│   ├── Scripting2Task/
│   │   ├── GameManager.cs
│   │   ├── PlayerMovement.cs
│   │   ├── PlayerInventory.cs
│   │   ├── MatchData.cs
│   │   ├── Interactable.cs
│   │   ├── KillZone.cs
│   │   ├── LevelCompleteZone.cs
│   │   └── UI/
│   │       ├── MainMenuUI.cs
│   │       ├── HeatBarUI.cs
│   │       ├── HeatTimerUI.cs
│   │       ├── MatchCounterUI.cs
│   │       ├── BurnTimerUI.cs
│   │       ├── InventoryUI.cs
│   │       ├── InventorySlotUI.cs
│   │       ├── DeathScreenUI.cs
│   │       ├── LightMatchPromptUI.cs
│   │       └── RestartButton.cs
├── ScriptableObjects/
│   ├── MatchData_Normal.asset
│   ├── MatchData_Bright.asset
│   ├── MatchData_Fragile.asset
│   └── MatchData_Special.asset
└── Prefabs/
    └── Slot.prefab
```
