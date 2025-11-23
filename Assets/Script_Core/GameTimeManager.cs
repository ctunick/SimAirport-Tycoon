using UnityEngine;

namespace SimAirport.Core {
    /// <summary>
    /// Manages simulation time (in minutes) and a local TimeScale for the sim clock.
    /// World speed (movement, spawning) is controlled separately via Time.timeScale,
    /// but TimeControls keeps them in sync.
    /// </summary>
    public class GameTimeManager : MonoBehaviour {
        /// <summary>
        /// Current simulation time in minutes.
        /// </summary>
        public float CurrentTimeMinutes { get; private set; }

        [Header("Start Time")]
        [Tooltip("Hour of day to start at, e.g. 8 = 08:00")]
        [SerializeField] private int startHour = 8;

        /// <summary>
        /// Simulation time scale (for the clock/HUD).
        /// </summary>
        public float TimeScale { get; private set; } = 1f;

        private void Start() {
            // Start the clock at startHour:00
            CurrentTimeMinutes = startHour * 60f;
        }

        private void Update() {
            if (TimeScale <= 0f)
                return;

            // Sim minutes scale with our own TimeScale.
            float deltaSimMinutes = Time.deltaTime * TimeScale;
            CurrentTimeMinutes += deltaSimMinutes;
        }

        /// <summary>
        /// Pause the sim clock (world pause is handled via Time.timeScale in TimeControls).
        /// </summary>
        public void Pause() {
            TimeScale = 0f;
        }

        /// <summary>
        /// Set simulation clock speed (0 = paused, 1 = normal, >1 = faster).
        /// </summary>
        public void SetSpeed(float scale) {
            TimeScale = Mathf.Clamp(scale, 0f, 10f);
        }

        // Helpers for HUD formatting

        public int CurrentHour =>
            Mathf.FloorToInt(CurrentTimeMinutes / 60f) % 24;

        public int CurrentMinute =>
            Mathf.FloorToInt(CurrentTimeMinutes) % 60;

        public int CurrentSecond =>
            Mathf.FloorToInt(CurrentTimeMinutes * 60f) % 60;
    }
}