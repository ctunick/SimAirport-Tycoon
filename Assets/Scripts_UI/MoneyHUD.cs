using UnityEngine;
using TMPro;
using SimAirport.Core;

namespace SimAirport.UI {
    /// <summary>
    /// Simple HUD that shows current cash as $X.
    /// Reads directly from EconomyManager.Instance.
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class MoneyHUD : MonoBehaviour {
        private TextMeshProUGUI _label;

        private void Awake() {
            _label = GetComponent<TextMeshProUGUI>();
        }

        private void Update() {
            if (_label == null)
                return;

            var economy = EconomyManager.Instance;
            if (economy == null) {
                _label.text = "$-";
                return;
            }

            _label.text = $"${economy.CurrentCash}";
        }
    }
}
