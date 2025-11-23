using SimAirport.Core;
using SimAirport.Economy;
using SimAirport.World;
using UnityEngine;

namespace SimAirport.Building {
	/// <summary>
	/// Implements the logic for building placement, connected to the economy and grid.
	/// </summary>
	public class BuildSystemService : MonoBehaviour {
		[SerializeField]
		private SimAirport.Core.GridManager gridManager;

		[SerializeField]
		private EconomyManager economyManager;

		public bool PlaceBuilding(BuildCommand command) {
			// TODO:
			// 1. Get BuildableDefinition from an asset registry/database.
			// 2. Check if player can afford it via EconomyManager.
			// 3. Check if the location is valid via GridManager.
			// 4. If all checks pass, spend money and place the building on the grid.
			return false;
		}
	}
}