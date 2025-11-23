using SimAirport.Core;
using UnityEngine;

namespace SimAirport.Building {
	/// <summary>
	/// Handles player input for placing buildings.
	/// </summary>
	public class BuildModeController : MonoBehaviour {
		private BuildableDefinition _selectedBuildable;

		private void Update() {
			if (_selectedBuildable == null) {
				return;
			}

			// TODO:
			// 1. Show a ghost preview of the buildable on the grid.
			// 2. On left-click:
			//    a. Create a BuildCommand.
			//    b. Send it to the CommandProcessor.
			//    c. On success, clear the selected buildable.
		}
	}
}