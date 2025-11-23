using UnityEngine;

namespace SimAirport.Core {
	/// <summary>
	/// Manages simulation time, speed, and pausing.
	/// </summary>
	public class GameTimeManager : MonoBehaviour {
		public float CurrentTimeMinutes { get; private set; }
		public float TimeScale { get; private set; } = 1.0f;

		// public static event Action<float> OnTick; // Example event

		private void Update() {
			if (TimeScale > 0) {
				float deltaSimMinutes = Time.deltaTime * TimeScale;
				CurrentTimeMinutes += deltaSimMinutes;
				// OnTick?.Invoke(deltaSimMinutes);
			}
		}

		public void Pause() => TimeScale = 0;

		public void SetSpeed(float scale) {
			TimeScale = Mathf.Clamp(scale, 0, 10);
		}
	}
}