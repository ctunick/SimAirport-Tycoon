using UnityEngine;

namespace SimAirport.Core {
    /// <summary>
    /// Central place to track player money.
    /// </summary>
    public class EconomyManager : MonoBehaviour {
        [Header("Starting Cash")]
        [SerializeField] private int startingCash = 0;

        /// <summary>
        /// Current amount of cash the player has.
        /// </summary>
        public int CurrentCash { get; private set; }

        /// <summary>
        /// Global access to the active EconomyManager.
        /// </summary>
        public static EconomyManager Instance { get; private set; }

        private void Awake() {
            // Simple singleton pattern: only one EconomyManager is allowed.
            if (Instance != null && Instance != this) {
                Debug.LogError("[EconomyManager] Multiple instances in scene, destroying duplicate.");
                Destroy(gameObject);
                return;
            }

            Instance = this;
            CurrentCash = startingCash;

            // Optional: keep this across scene loads (fine for now)
            // DontDestroyOnLoad(gameObject);

            Debug.Log($"[EconomyManager] Initialized with starting cash = {CurrentCash}");
        }

        /// <summary>
        /// Add positive or negative cash (negative = spend).
        /// </summary>
        public void AddCash(int amount) {
            if (amount == 0) return;

            CurrentCash += amount;
            Debug.Log($"[Money] +{amount}. Total: {CurrentCash}");
        }

        /// <summary>
        /// Try to spend some cash. Returns true if successful.
        /// </summary>
        public bool TrySpendCash(int amount) {
            if (amount < 0) {
                Debug.LogWarning("[EconomyManager] TrySpendCash called with negative amount.");
                return false;
            }

            if (CurrentCash < amount) {
                Debug.Log("[Money] Not enough cash.");
                return false;
            }

            AddCash(-amount);
            return true;
        }
    }
}