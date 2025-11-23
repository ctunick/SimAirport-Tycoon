using UnityEngine;

namespace SimAirport.Core {
	public enum GameState {
		MainMenu,
		Playing,
		Paused
	}

	/// <summary>
	/// Manages the overall state of the game (e.g., main menu, playing, paused).
	/// </summary>
	public class GameStateManager : MonoBehaviour {
		public GameState CurrentState { get; private set; }
		// TODO: Add methods for state transitions and expose events.
	}
}