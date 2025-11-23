using UnityEngine;
using UnityEngine.AI;

public class PassengerNavTest : MonoBehaviour {
    public Transform gatePoint;
    private NavMeshAgent agent;

    void Awake() {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start() {
        if (gatePoint == null) {
            Debug.LogError("GatePoint not assigned on PassengerNavTest.", this);
            return;
        }

        // Tell the agent to walk to the gate
        agent.SetDestination(gatePoint.position);
    }
}
