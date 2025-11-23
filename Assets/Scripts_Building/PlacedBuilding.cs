using UnityEngine;

namespace SimAirport.Building {
	/// <summary>
	/// Represents an instance of a placed building in the world.
	/// </summary>
	public class PlacedBuilding : MonoBehaviour {
		public BuildableDefinition Definition { get; private set; }
		public Vector2Int GridOrigin { get; private set; }
		public int Rotation { get; private set; }

		// TODO: Add initialization method
	}
}