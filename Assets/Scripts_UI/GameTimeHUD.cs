using UnityEngine;
using TMPro;
using SimAirport.Core;

namespace SimAirport.UI {
    /// <summary>
    /// Tiny debug HUD that shows the current game time as HH:MM.
    /// - Visible in Editor / DEV_BUILD / TEST_BUILD.
    /// - Hidden in production builds.
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class GameTimeHUD : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI timeLabel;
        [SerializeField] private GameTimeManager timeManager;

        private void Awake() {
            // Always grab the TMP Text on this object if not wired
            if (timeLabel == null)
                timeLabel = GetComponent<TextMeshProUGUI>();

#if !(UNITY_EDITOR || DEV_BUILD || TEST_BUILD)
            // PRODUCTION BUILD:
            // Hide label and disable component so nothing is shown.
            if (timeLabel != null)
                timeLabel.enabled = false;

            enabled = false;
            return;
#endif

            // EDITOR / DEV / TEST:
            if (timeLabel != null) {
                timeLabel.enabled = true;
                timeLabel.text = "Time HUD starting…";
            }

            if (timeManager == null) {
                timeManager = FindObjectOfType<GameTimeManager>();
                if (timeManager == null) {
                    Debug.LogWarning("[GameTimeHUD] No GameTimeManager found in scene.");
                    if (timeLabel != null)
                        timeLabel.text = "No GameTimeManager";
                }
            }
        }

        private void Update() {
            if (!enabled || timeLabel == null || timeManager == null)
                return;

            int hour = timeManager.CurrentHour;
            int minute = timeManager.CurrentMinute;

            timeLabel.text = $"{hour:00}:{minute:00}";
        }
    }
}