using UnityEngine;

namespace SimAirport.AI {
	/// <summary>
	/// Responsible for spawning passenger agents at entrance zones.
	/// </summary>
	public class PassengerSpawner : MonoBehaviour {
		[SerializeField]
		private GameObject passengerPrefab;

		public void SpawnPassenger() {
			// TODO:
			// 1. Find an available entrance zone from ZoneRegistry.
			// 2. Instantiate passengerPrefab at the zone's entry point.
			// 3. Initialize the PassengerAgent component.
		}
	}
}