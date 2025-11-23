using UnityEngine;

namespace SimAirport.Core {
	/// <summary>
	/// Processes commands to modify game state. This is key for future co-op support.
	/// </summary>
	public class CommandProcessor : MonoBehaviour {
		// In a real implementation, this would trigger events or call other services.

		public bool Apply(BuildCommand cmd) {
			Debug.Log($"Processing BuildCommand for {cmd.buildableId} at ({cmd.gridX}, {cmd.gridY})");
			// TODO: Connect to BuildSystemService
			return true;
		}

		public bool Apply(DeleteCommand cmd) {
			Debug.Log($"Processing DeleteCommand for instance {cmd.buildingInstanceId}");
			// TODO: Connect to a service to remove the building
			return true;
		}
	}
}