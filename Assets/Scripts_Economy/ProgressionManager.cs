using System.Collections.Generic;
using UnityEngine;

namespace SimAirport.Economy {
	/// <summary>
	/// Manages unlocking new buildables based on game progress.
	/// </summary>
	public class ProgressionManager : MonoBehaviour {
		private List<UnlockRule> _unlockRules;
		private HashSet<string> _unlockedBuildableIds = new HashSet<string>();

		private void Start() {
			// TODO: Load unlock rules from JSON data.
			// TODO: Subscribe to EconomyManager events to check rules.
		}

		public bool IsUnlocked(string buildableId) {
			return _unlockedBuildableIds.Contains(buildableId);
		}
	}

	[System.Serializable]
	public class UnlockRule {
		public string UnlockId;
		public string BuildableId;
		public int MinPassengersServed;
	}
}