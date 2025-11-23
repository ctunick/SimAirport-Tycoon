namespace SimAirport.Core {
	/// <summary>
	/// Provides unique IDs for game entities.
	/// </summary>
	public static class IdGenerator {
		private static int _nextPassengerId = 0;
		private static int _nextBuildingId = 0;

		public static int GetNextPassengerId() => _nextPassengerId++;
		public static int GetNextBuildingId() => _nextBuildingId++;
	}
}