using SimAirport.World;
using UnityEngine;
using UnityEngine.AI;

namespace SimAirport.AI {
	public enum PassengerState {
		Spawned,
		ToEntrance,
		ToCheckIn,
		ToSecurity,
		ToGate,
		Boarding,
		Done
	}

	/// <summary>
	/// Represents a single passenger moving through the airport.
	/// </summary>
	[RequireComponent(typeof(NavMeshAgent))]
	public class PassengerAgent : MonoBehaviour {
		public int PassengerId { get; private set; }
		public PassengerState CurrentState { get; private set; }

		private NavMeshAgent _navAgent;
		private ZoneComponent _currentTarget;

		private void Awake() {
			_navAgent = GetComponent<NavMeshAgent>();
		}

		public void Initialize(int id) {
			PassengerId = id;
			CurrentState = PassengerState.Spawned;
			// TODO: Start state machine
		}

		private void Update() {
			// TODO: Implement state machine logic
			// - Find next zone using ZoneRegistry
			// - Move to zone
			// - Simulate service time and transition to next state
		}
	}
}