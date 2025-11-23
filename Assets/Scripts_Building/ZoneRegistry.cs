using System.Collections.Generic;
using UnityEngine;

namespace SimAirport.World {
	/// <summary>
	/// A singleton service that keeps track of all available zones in the airport.
	/// </summary>
	public class ZoneRegistry : MonoBehaviour {
		// TODO: Implement as a proper singleton
		public static ZoneRegistry Instance;

		private Dictionary<ZoneType, List<ZoneComponent>> _zones = new Dictionary<ZoneType, List<ZoneComponent>>();

		private void Awake() {
			Instance = this;
		}

		public void Register(ZoneComponent zone) {
			// TODO: Add zone to the dictionary
		}

		public ZoneComponent GetNearestZone(ZoneType type, Vector3 fromPosition) {
			// TODO: Find and return the closest zone of the requested type.
			return null;
		}
	}
}