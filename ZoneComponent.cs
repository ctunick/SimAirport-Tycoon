using UnityEngine;

namespace SimAirport.World {
	/// <summary>
	/// Defines an interactable area for passengers. Attached to building prefabs.
	/// </summary>
	public class ZoneComponent : MonoBehaviour {
		public ZoneType zoneType;
		public Transform entryPoint;
		public int capacity = 4;

		public Vector3 GetEntryPoint() => entryPoint.position;
	}
}