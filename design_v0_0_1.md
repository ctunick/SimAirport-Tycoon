# SimAirport Tycoon - v0.0.1 Design

This document outlines the features and systems for the initial vertical slice (v0.0.1).

## 1. Core Gameplay Loop

The primary loop for v0.0.1 is:

**Build -> Simulate -> Earn -> Unlock**

-   **Build:** The player places essential airport facilities on a grid.
-   **Simulate:** AI passengers spawn and navigate through the placed facilities.
-   **Earn:** The player earns money for each passenger that successfully boards a flight.
-   **Unlock:** Reaching milestones (like passengers served) unlocks new buildables.

## 2. System Architecture

### Unity (C#)

-   **Core Systems:** Manages game state, time, and player commands.
-   **World & Building:** Handles the grid system and placement of buildable objects.
-   **AI:** Manages passenger spawning, state machines, and movement (NavMesh).
-   **Economy & Progression:** Tracks finances and unlocks new content.
-   **UI:** Displays game information and provides build controls.

### Python

-   **Data Generation:** Python scripts generate flight schedules and progression rules.
-   **Export:** Data is exported as JSON files, which are consumed by Unity at runtime. This keeps game data separate from code and allows for easier balancing and modding.