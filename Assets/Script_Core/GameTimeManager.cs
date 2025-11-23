using UnityEngine;
using System;

namespace SimAirport.Core {
	/// <summary>
	/// Manages simulation time, speed, and pausing.
	/// Tracks time in minutes since midnight and advances every frame.
	/// </summary>
	public class GameTimeManager : MonoBehaviour {
		[Header("Initial Time")]
		[Tooltip("Initial time of day in minutes since midnight (e.g. 480 = 08:00).")]
		[SerializeField] private float startTimeMinutes = 8f * 60f; // 08:00

		[Header("Simulation Speed")]
		[Tooltip("Base in-game minutes that pass per real-time second when TimeScale = 1.")]
		[SerializeField] private float minutesPerSecond = 1f;

		/// <summary>
		/// Current simulation time, in minutes since midnight.
		/// Visible in Inspector for debugging.
		/// </summary>
		[field: SerializeField]
		public float CurrentTimeMinutes { get; private set; }

		/// <summary>
		/// Multiplier applied on top of minutesPerSecond (0 = paused).
		/// </summary>
		public float TimeScale { get; private set; } = 1.0f;

		/// <summary>
		/// Convenience property for current hour (0–23).
		/// </summary>
		public int CurrentHour => (int)(CurrentTimeMinutes / 60f) % 24;

		/// <summary>
		/// Convenience property for current minute (0–59).
		/// </summary>
		public int CurrentMinute => (int)(CurrentTimeMinutes % 60f);

		// Optional tick event if other systems want delta minutes.
		// public static event Action<float> OnTick;

#if UNITY_EDITOR || DEV_BUILD || TEST_BUILD
		[Header("Debug")]
		[SerializeField] private bool logTimeEverySecond = false;

		private float _logTimer;
#endif

		private void Awake() {
			// Initialize sim time to the configured start.
			CurrentTimeMinutes = startTimeMinutes;
		}

		private void Update() {
			if (TimeScale <= 0f || minutesPerSecond <= 0f)
				return;

			float deltaSimMinutes = Time.deltaTime * TimeScale * minutesPerSecond;
			CurrentTimeMinutes += deltaSimMinutes;

			// Wrap around at 24 hours to keep numbers reasonable.
			if (CurrentTimeMinutes >= 24f * 60f)
				CurrentTimeMinutes -= 24f * 60f;

			// OnTick?.Invoke(deltaSimMinutes);

#if UNITY_EDITOR || DEV_BUILD || TEST_BUILD
			// Optional debug log once per real-time second.
			if (logTimeEverySecond) {
				_logTimer += Time.deltaTime;
				if (_logTimer >= 1f) {
					_logTimer = 0f;
					Debug.Log($"[GameTime] {CurrentHour:00}:{CurrentMinute:00} ({CurrentTimeMinutes:F1} mins)");
				}
			}
#endif
		}

		/// <summary>
		/// Pause the simulation.
		/// </summary>
		public void Pause() => TimeScale = 0f;

		/// <summary>
		/// Resume the simulation at normal speed (TimeScale = 1).
		/// </summary>
		public void Resume() => TimeScale = 1f;

		/// <summary>
		/// Set the simulation speed multiplier.
		/// </summary>
		public void SetSpeed(float scale) {
			TimeScale = Mathf.Clamp(scale, 0f, 10f);
		}

		/// <summary>
		/// Set the current time of day (in minutes since midnight).
		/// </summary>
		public void SetTimeMinutes(float minutes) {
			CurrentTimeMinutes = Mathf.Repeat(minutes, 24f * 60f);
		}
	}
}
