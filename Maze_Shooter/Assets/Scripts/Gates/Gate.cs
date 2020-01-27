using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class Gate : MonoBehaviour
{
    public Stage destination;
    [Tooltip("GateLinks are shared between pairs of gates. This lets the game know which gate in the destination stage" +
             " the player will start at.")]
    public GateLink gateLink;
    public Vector3 gateForward = Vector3.forward;

    public UnityEvent onPlayerEnterGate;
    public UnityEvent onPlayerSpawnToThisGate;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.rotation * gateForward * 5);
    }

    // Start is called before the first frame update
    void Start()
    {}

    void SpawnPlayer()
    {
        onPlayerSpawnToThisGate.Invoke();
    }


    public void TryTransportPlayer()
    {
        if (!enabled) return;
        // TODO check if the player is moving the right way through the gate using Vector3.Dot against gateForward
        GameMaster.SetGateLink(gateLink);
        onPlayerEnterGate.Invoke();
    }

    public void LoadDestination()
    {
        destination.Load(0);
    }

    public void CheckIfSpawningPlayer()
    {
        if (GameMaster.Get().gateLink == gateLink)
            onPlayerSpawnToThisGate.Invoke();
    }
}
