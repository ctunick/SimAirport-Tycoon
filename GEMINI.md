You are an expert Unity (C#) and Python game engineer helping me build v0.0.1 of an open-source “Airport Tycoon” management game for PC.

GOAL
-----
Create the initial repo + project structure and implement a minimal but extensible vertical slice of the game that includes:
- A small buildable airport world on a grid.
- Basic building placement (entrance, check-in, security, gate, restroom).
- Simple passenger AI agents that spawn, walk through these zones, and board flights.
- A simple economy (money from successfully boarded passengers, building costs).
- A basic progression/unlock system (new buildables unlock as milestones are met).
- Single-player support now, but structured so co-op/multiplayer can be added later.
- Python tools to generate data (flight schedules, progression configs) consumed by Unity.

HIGH-LEVEL ARCHITECTURE
------------------------
1. Unity (C#) - primary runtime game:
   - Rendering, input, UI.
   - Grid/world system and building placement.
   - Passenger AI using NavMesh.
   - Economy and progression.
   - Save/load stubs.

2. Python - tools and data pipelines:
   - Scripts to generate flight schedules and progression data.
   - Export JSON to be read by Unity.
   - Designed so additional tools (balancing, AI experiments) can be added later.

REPO STRUCTURE
----------------
Create this folder layout:

airport-tycoon/
  LICENSE               # placeholder (MIT)
  README.md             # basic project overview and build instructions
  docs/
    design_v0_0_1.md    # describe scope and systems
    CONTRIBUTING.md     # simple contributor guide
  unity/
    AirportTycoonUnity/           # Unity project
      Assets/
        Scripts/
          Core/
          World/
          Building/
          AI/
          Economy/
          UI/
        Data/                     # JSON configs from Python
        Art/
          Placeholders/
      ProjectSettings/
      Packages/
  python/
    data_generators/
      flight_schedule_generator.py
      passenger_profile_generator.py
    tools/
      export_to_json.py
      balance_report.py
    config/
      example_airport_config.yaml
    README.md

UNITY IMPLEMENTATION DETAILS
-----------------------------
Implement the following Unity systems and scripts.

1) Core systems (Assets/Scripts/Core)

- GameTimeManager:
  - Public properties: CurrentTimeMinutes, TimeScale (0, 1, 3).
  - Public methods: Pause(), SetSpeed(float scale).
  - Raises an event or uses a C# delegate OnTick(float deltaSimMinutes).
  - Uses Unity’s Update() to accumulate real time and convert to sim time.

- GameStateManager:
  - Enum GameState { MainMenu, Playing, Paused }.
  - Handles transitions, new game initialization, and exposes events OnGameStarted, OnGamePaused, etc.

- Command system for future co-op:
  - Define serializable command structs in a Commands.cs file, e.g.:
    - BuildCommand { string playerId; string buildableId; int gridX; int gridY; int rotation; }
    - DeleteCommand { string playerId; int buildingInstanceId; }
  - CommandProcessor:
    - Method Apply(BuildCommand cmd) and Apply(DeleteCommand cmd).
    - Returns success/failure and a reason.
    - All build/delete actions in-game must pass through this layer even in single player.

- IdGenerator:
  - Provides unique integer IDs for passengers and buildings.

2) World & grid (Assets/Scripts/World and Assets/Scripts/Building)

- GridManager (World):
  - Configurable grid size (width, height).
  - Methods:
    - bool IsCellFree(Int2 cell);
    - bool CanPlace(BuildableDefinition def, Int2 origin);
    - PlacedBuilding PlaceBuilding(BuildableDefinition def, Int2 origin, int rotation);
    - void RemoveBuilding(PlacedBuilding building);
  - Use an internal 2D array or dictionary to track occupancy.

- BuildableDefinition (Building, ScriptableObject):
  - Fields:
    - string Id;
    - string DisplayName;
    - int Cost;
    - int Width;
    - int Height;
    - BuildCategory Category (Structure, Facility, Decoration);
    - ZoneType ProvidedZoneType (None, Entrance, CheckIn, Security, Gate, Restroom, Exit).
    - UnlockConditionId (string key referencing progression rules).

- PlacedBuilding (Building, MonoBehaviour):
  - Holds reference to BuildableDefinition.
  - Stores grid origin and rotation.
  - Optionally contains a ZoneComponent if this building provides a passenger interaction zone.

3) Zones & registry (Assets/Scripts/World)

- Enum ZoneType { None, Entrance, CheckIn, Security, Gate, Restroom, Exit }.

- ZoneComponent (MonoBehaviour):
  - Fields:
    - ZoneType zoneType;
    - Transform entryPoint;
    - int capacity;  // keep simple for now.
  - Expose GetEntryPoint().

- ZoneRegistry (singleton service):
  - Maintains lists of ZoneComponent by ZoneType.
  - Methods:
    - ZoneComponent GetNearestZone(ZoneType type, Vector3 fromPosition).

4) Passenger AI (Assets/Scripts/AI)

- PassengerAgent (MonoBehaviour):
  - Fields:
    - int PassengerId;
    - NavMeshAgent navAgent;
    - PassengerState state enum: Spawned, ToCheckIn, ToSecurity, ToGate, Boarding, Done;
    - ZoneComponent currentTarget;
  - Methods:
    - Initialize(PassengerSpawnContext ctx, int id) to set initial state.
    - SetDestination(Vector3 position).
    - UpdateStateMachine() called from Update().
    - When reaching a zone, simulate a fixed service time before progressing to the next state.

- PassengerSpawner:
  - Reads a flight schedule JSON from Assets/Data.
  - Uses GameTimeManager.CurrentTimeMinutes to decide when to spawn passengers.
  - Spawns PassengerAgent prefabs at Entrance zones.
  - Keep spawn logic simple: for v0.0.1, just spawn a small number of passengers per in-game hour, or tied to a single “demo” flight.

- FlightManager:
  - Load a simple flight schedule from JSON (flight_id, time_minutes, gate_id, passenger_count).
  - For v0.0.1, it can:
    - Trigger passenger spawns associated with each flight.
    - Trigger simple plane arrival/departure visuals at the assigned gate building.

5) Economy & progression (Assets/Scripts/Economy)

- EconomyManager:
  - Fields:
    - int currentCash;
    - int totalRevenue;
    - int totalSpent;
    - int passengersServed;
  - Methods:
    - bool CanAfford(int cost);
    - void Spend(int cost);
    - void AddRevenue(int amount);
    - void OnPassengerBoarded(PassengerAgent passenger) which:
      - increments passengersServed;
      - calls AddRevenue(feePerPassenger).
  - Expose events or C# actions for UI updates (OnCashChanged, OnPassengersServedChanged).

- ProgressionManager:
  - Data-driven: load a list of UnlockRule objects from JSON.
    - UnlockRule fields:
      - string UnlockId;
      - string BuildableId;
      - int MinPassengersServed;
      - int MinRevenue;
  - On relevant events (cash or passengersServed changes), evaluate rules and unlock buildables.
  - Provide method:
    - bool IsUnlocked(string buildableId).

6) Build mode & UI (Assets/Scripts/UI and Assets/Scripts/Building)

- BuildModeController:
  - Handles player input for building placement.
  - When a buildable is selected from UI:
    - Shows a ghost preview on the grid using raycasts.
    - On left-click, constructs a BuildCommand and sends it to CommandProcessor.
  - On successful placement, spawns the appropriate prefab and updates GridManager.

- BuildSystemService:
  - Private reference to GridManager and EconomyManager.
  - Implements the actual build logic:
    - Verify CanAfford and CanPlace.
    - Call EconomyManager.Spend().
    - Call GridManager.PlaceBuilding().
    - Hook in NavMeshSurface updates if needed.

- Basic UI:
  - Top bar showing current cash, time, passengers served, and speed controls.
  - A simple build panel listing unlocked buildables by category.
  - Lock icons and tooltips for locked buildables referencing unlock requirements.

7) NavMesh and visuals

- Use Unity’s NavMesh system:
  - Add NavMeshSurface components to floor/terrain.
  - Rebuild NavMesh when buildings change (for v0.0.1 a simple full rebake or batched updates is sufficient).

- Visuals:
  - Use Unity primitives or very simple placeholder prefabs (cubes, quads, icons) for:
    - Buildings.
    - Passengers (capsule + color).
    - Planes (box with wings).

PYTHON IMPLEMENTATION
----------------------
Under python/data_generators and python/tools, implement:

1) flight_schedule_generator.py
   - Generates a simple list of flights for a day:
     - Random or evenly spaced times (e.g., every 2 in-game hours).
     - For v0.0.1, support a single gate: "GateA".
   - Output format (JSON) like:
     {
       "airport_id": "default_small",
       "flight_schedule": [
         {"flight_id": "AT100", "time_minutes": 60, "gate": "GateA", "passengers": 20},
         {"flight_id": "AT101", "time_minutes": 120, "gate": "GateA", "passengers": 30}
       ]
     }

2) passenger_profile_generator.py
   - For v0.0.1, just stub basic profiles:
     - age_group, travel_purpose, walking_speed_modifier.
   - Generate a small list and save to JSON.

3) export_to_json.py
   - Helper functions to:
     - Read python/config/example_airport_config.yaml.
     - Call the generator functions.
     - Write JSON files into unity/AirportTycoonUnity/Assets/Data/.

4) balance_report.py
   - Optional: read configs and print simple stats:
     - total flights per day,
     - expected passenger count,
     - potential revenue given the per-passenger fee.

Design these scripts so they can be run via:
- `python export_to_json.py` from the python/ directory.

CO-OP FUTURE-PROOFING
----------------------
Even though we implement only single-player now, adhere to these patterns:

- All player actions (build, delete) must go through the CommandProcessor and the serialized command types.
- Entities (buildings, passengers) must have unique IDs via IdGenerator.
- Keep randomness controlled via one RandomManager with a configurable seed.
- Write code so the simulation core (economy, passenger state transitions, command processing) is decoupled from direct UI calls. Ideally, only Unity MonoBehaviours handle visuals and inputs, while pure C# classes (POCOs) handle the core sim logic.

DOCS & OPEN SOURCE
-------------------
- In docs/design_v0_0_1.md, document:
  - Feature scope,
  - Major systems and their responsibilities,
  - How data flows from Python -> JSON -> Unity.
- In README.md:
  - List prerequisites (Unity version, Python version).
  - Basic “getting started” steps:
    - How to open the Unity project.
    - How to run the Python scripts to generate data.
- Use an MIT license file with a placeholder copyright.

DELIVERABLES
-------------
1. Create all the described folders/scripts/classes.
2. Implement minimal working logic for:
   - Grid placement,
   - Passenger spawning and movement between zones,
   - Economy increments when passengers board,
   - Unlocking at least one new buildable based on passengersServed.
3. Provide inline comments and brief XML doc comments for the key public methods and classes.
4. Add at least one Unity scene “DemoScene” that:
   - Contains a basic airport floor,
   - Has pre-placed Entrance, CheckIn, Security, Gate, Restroom buildings,
   - Spawns passengers based on a small flight schedule JSON.
5. Ensure the project is buildable to a Windows standalone executable in Unity.

Start by scaffolding the repo structure and empty files, then fill in the C# and Python implementations step by step.
