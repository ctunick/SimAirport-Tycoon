namespace SimAirport.Core {
	[System.Serializable]
	public struct BuildCommand {
		public string playerId;
		public string buildableId;
		public int gridX;
		public int gridY;
		public int rotation;
	}

	[System.Serializable]
	public struct DeleteCommand {
		public string playerId;
		public int buildingInstanceId;
	}
}