using SimAirport.World;
using SimAirport.Core;
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
	/// For the initial walking skeleton:
	/// - Uses debug waypoints (Entrance -> Gate).
	/// - Walks along the NavMesh to the gate.
	/// - On arrival, boards and adds ticket revenue to the EconomyManager.
	/// </summary>
	[RequireComponent(typeof(NavMeshAgent))]
	public class PassengerAgent : MonoBehaviour {
		#region Public API

		public int PassengerId { get; private set; }
		public PassengerState CurrentState { get; private set; }

		/// <summary>
		/// Initialize from a spawner / higher-level system.
		/// Later this will wire in ZoneComponent targets (check-in, security, gate, etc.).
		/// For now, if debug waypoints are assigned, we immediately start the
		/// Entrance -> Gate flow.
		/// </summary>
		public void Initialize(int id, ZoneComponent initialTarget = null) {
			PassengerId = id;
			_initializedFromSpawner = true;

			_currentTarget = initialTarget;
			CurrentState = PassengerState.Spawned;

			// For now, use the simple Entrance -> Gate flow if debug points are set.
			if (_entrancePoint != null && _gatePoint != null) {
				StartToGateFlow();
			} else {
				Debug.LogWarning($"[Passenger {PassengerId}] Initialized without debug waypoints. " +
				                 "State machine will be idle until proper zone routing is implemented.", this);
			}
		}

		public void InitializeForDebug(int id, Transform entrancePoint, Transform gatePoint) {
    		_entrancePoint = entrancePoint;
    		_gatePoint = gatePoint;
    		Initialize(id);
}


		#endregion

		#region Serialized Fields

		[Header("Debug Waypoints (Temporary Walking Skeleton)")]
		[Tooltip("Where the passenger starts for this simple vertical slice.")]
		[SerializeField] private Transform _entrancePoint;

		[Tooltip("Where the passenger should walk to (simple gate stand-in).")]
		[SerializeField] private Transform _gatePoint;

		[Tooltip("How close to the destination the agent must be to count as 'arrived'.")]
		[SerializeField] private float _arrivalTolerance = 0.1f;

		[Header("Economy")]
		[Tooltip("How much revenue this passenger generates when boarding.")]
		[SerializeField] private int _ticketRevenue = 10;

		#endregion

		#region Private Fields

		private NavMeshAgent _navAgent;
		private ZoneComponent _currentTarget;

		private bool _initializedFromSpawner;
		private bool _hasBoarded;

		// Simple local ID generator for editor/dev usage if Initialize() is never called.
		private static int _nextDebugId = 1;

		#endregion

		#region Unity Lifecycle

		private void Awake() {
			_navAgent = GetComponent<NavMeshAgent>();
		}

		private void Start() {
			// If no external system called Initialize(), fall back to a simple
			// editor-friendly setup using the debug waypoints.
			if (!_initializedFromSpawner && _entrancePoint != null && _gatePoint != null) {
				PassengerId = _nextDebugId++;
				StartToGateFlow();
			}
		}

		private void Update() {
			switch (CurrentState) {
				case PassengerState.ToGate:
					UpdateToGate();
					break;

				// Future states will be fleshed out later:
				// case PassengerState.ToEntrance:
				// case PassengerState.ToCheckIn:
				// case PassengerState.ToSecurity:
				// case PassengerState.Boarding:
				//	   ...
				// case PassengerState.Done:
				//	   break;
			}
		}

		#endregion

		#region State Machine: Simple Entrance -> Gate Flow

		/// <summary>
		/// Begin the simple debug flow: warp to Entrance and walk to Gate.
		/// </summary>
		private void StartToGateFlow() {
			if (_navAgent == null) {
				Debug.LogError("[PassengerAgent] NavMeshAgent missing.", this);
				enabled = false;
				return;
			}

			if (_entrancePoint == null || _gatePoint == null) {
				Debug.LogError("[PassengerAgent] Entrance or GatePoint not assigned.", this);
				enabled = false;
				return;
			}

			// Start at entrance and sync with NavMesh.
			transform.position = _entrancePoint.position;
			_navAgent.Warp(_entrancePoint.position);

			_navAgent.SetDestination(_gatePoint.position);

			CurrentState = PassengerState.ToGate;

			Debug.Log($"[Passenger {PassengerId}] Walking from Entrance to Gate.");
		}

		/// <summary>
		/// Per-frame update while walking to the gate.
		/// </summary>
		private void UpdateToGate() {
			if (_hasBoarded || _navAgent == null)
				return;

			if (_navAgent.pathPending)
				return;

			// More forgiving arrival check
			if (_navAgent.remainingDistance <= _navAgent.stoppingDistance + _arrivalTolerance) {
				OnArrivedAtGate();
			}
		}


		/// <summary>
		/// Called once when the passenger reaches the gate.
		/// </summary>
		private void OnArrivedAtGate() {
			if (_hasBoarded)
				return;

			_hasBoarded = true;
			CurrentState = PassengerState.Boarding;

			// Stop moving and clear the path
			if (_navAgent != null) {
				_navAgent.isStopped = true;
				_navAgent.ResetPath();
			}

			Debug.Log($"[Passenger {PassengerId}] Boarded at gate.");

			// Simple economy hook for this vertical slice.
			if (EconomyManager.Instance != null) {
				EconomyManager.Instance.AddCash(_ticketRevenue);
			} else {
				Debug.LogWarning("[PassengerAgent] No EconomyManager found; ticket revenue not applied.");
			}

			CurrentState = PassengerState.Done;

			// Despawn this passenger after a short delay so the gate is clear
			Destroy(gameObject, 0.5f);
		}


		#endregion

		#region Editor Helpers

#if UNITY_EDITOR
		private void OnDrawGizmosSelected() {
			if (_entrancePoint != null) {
				Gizmos.color = Color.green;
				Gizmos.DrawSphere(_entrancePoint.position, 0.2f);
			}

			if (_gatePoint != null) {
				Gizmos.color = Color.cyan;
				Gizmos.DrawSphere(_gatePoint.position, 0.2f);
			}

			if (_entrancePoint != null && _gatePoint != null) {
				Gizmos.color = Color.yellow;
				Gizmos.DrawLine(_entrancePoint.position, _gatePoint.position);
			}
		}
#endif

		#endregion
	}
}
