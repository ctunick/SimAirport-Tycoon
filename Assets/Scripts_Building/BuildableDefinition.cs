using SimAirport.World;
using UnityEngine;

namespace SimAirport.Building {
	public enum BuildCategory {
		Structure,
		Facility,
		Decoration
	}

	[CreateAssetMenu(fileName = "NewBuildable", menuName = "SimAirport/Buildable Definition")]
	public class BuildableDefinition : ScriptableObject {
		public string Id;
		public string DisplayName;
		public int Cost;
		public int Width = 1;
		public int Height = 1;
		public BuildCategory Category;
		public ZoneType ProvidedZoneType = ZoneType.None;
		public string UnlockConditionId;
	}
}