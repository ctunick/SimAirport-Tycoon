using UnityEngine;
using SimAirport.Core;

namespace SimAirport.Core {
    /// <summary>
    /// Keyboard controls for simulation time:
    /// - Space: toggle pause / resume (remembers last non-zero speed)
    /// - 1: set speed to 1x
    /// - 2: set speed to 3x
    /// - 3: set speed to 5x
    ///
    /// These affect BOTH:
    /// - Sim clock (GameTimeManager.TimeScale)
    /// - World (Unity's Time.timeScale: movement, spawning, etc.)
    /// </summary>
    public class TimeControls : MonoBehaviour {
        [SerializeField] private GameTimeManager timeManager;

        private float _lastNonZeroSpeed = 1f;

        private void Awake() {
            if (timeManager == null) {
                timeManager = FindObjectOfType<GameTimeManager>();
                if (timeManager == null) {
                    Debug.LogError("[TimeControls] No GameTimeManager found in scene.");
                }
            }

            if (timeManager != null && timeManager.TimeScale > 0f) {
                _lastNonZeroSpeed = timeManager.TimeScale;
            }

            // Make sure Unity's global time starts sane
            if (Mathf.Approximately(Time.timeScale, 0f)) {
                Time.timeScale = 1f;
            }
        }

        private void Update() {
            if (timeManager == null)
                return;

            HandlePauseToggle();
            HandleSpeedKeys();
        }

        private void HandlePauseToggle() {
            if (!Input.GetKeyDown(KeyCode.Space))
                return;

            if (Mathf.Approximately(Time.timeScale, 0f)) {
                // Currently paused -> resume at last known non-zero speed
                if (_lastNonZeroSpeed <= 0f) {
                    _lastNonZeroSpeed = 1f;
                }

                SetGlobalSpeed(_lastNonZeroSpeed);
            } else {
                // Currently running -> remember speed and pause
                _lastNonZeroSpeed = Mathf.Max(timeManager.TimeScale, 1f);

                timeManager.Pause();     // sim clock
                Time.timeScale = 0f;     // world
            }
        }

        private void HandleSpeedKeys() {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                SetGlobalSpeed(1f); // 1x
            }

            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                SetGlobalSpeed(3f); // 3x
            }

            if (Input.GetKeyDown(KeyCode.Alpha3)) {
                SetGlobalSpeed(5f); // 5x
            }
        }

        private void SetGlobalSpeed(float speed) {
            _lastNonZeroSpeed = speed;

            // Sim clock
            timeManager.SetSpeed(speed);

            // World (movement, spawning, etc.)
            Time.timeScale = speed;
        }
    }
}