using UnityEngine;

namespace SimAirport.Building
{
    [CreateAssetMenu(
        fileName = "BuildableDefinition",
        menuName = "SimAirport/Buildable Definition",
        order = 0)]
    public class BuildableDefinition : ScriptableObject
    {
        [Header("Basic Info")]
        public string id;              // e.g. "gate_basic"
        public string displayName;     // e.g. "Gate (Basic)"

		[Header("Prefab")]
		public GameObject prefab;      // The prefab to spawn
    // The prefab to spawn

        [Header("Grid Footprint")]
        public int width = 1;          // cells in X
        public int height = 1;         // cells in Y (grid)
    }
}