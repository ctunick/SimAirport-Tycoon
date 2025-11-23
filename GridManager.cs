using SimAirport.Building;
using UnityEngine;

namespace SimAirport.World {
	/// <summary>
	/// Manages the world grid and building occupancy.
	/// </summary>
	public class GridManager : MonoBehaviour {
		[SerializeField]
		private int width = 100;
		[SerializeField]
		private int height = 100;

		// TODO: Use a 2D array or dictionary to track occupied cells.

		public bool IsCellFree(Vector2Int cell) {
			// TODO: Implement check
			return true;
		}

		public PlacedBuilding PlaceBuilding(BuildableDefinition def, Vector2Int origin, int rotation) {
			// TODO: Implement placement logic
			return null;
		}
	}
}