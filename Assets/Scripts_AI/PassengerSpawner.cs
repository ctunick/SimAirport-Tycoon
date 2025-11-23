using UnityEngine;
using SimAirport.AI;

namespace SimAirport.AI {
    /// <summary>
    /// Very simple spawner for the walking skeleton:
    /// spawns a passenger every X real seconds and sends them Entrance -> Gate.
    /// </summary>
    public class PassengerSpawner : MonoBehaviour {
        [Header("Setup")]
        [SerializeField] private PassengerAgent passengerPrefab;
        [SerializeField] private Transform entrancePoint;
        [SerializeField] private Transform gatePoint;

        [Header("Spawn Timing (real seconds for now)")]
        [SerializeField] private float spawnIntervalSeconds = 5f;

        private float _timer;
        private int _nextPassengerId = 1;

        private void Update() {
            _timer += Time.deltaTime;
            if (_timer >= spawnIntervalSeconds) {
                _timer -= spawnIntervalSeconds;
                SpawnPassenger();
            }
        }

        private void SpawnPassenger() {
            if (passengerPrefab == null || entrancePoint == null || gatePoint == null) {
                Debug.LogError("[PassengerSpawner] Missing references.", this);
                return;
            }

            // Create a new passenger and initialize it with our debug waypoints
            PassengerAgent agent = Instantiate(
                passengerPrefab,
                entrancePoint.position,
                Quaternion.identity
            );

            agent.InitializeForDebug(_nextPassengerId++, entrancePoint, gatePoint);
        }
    }
}