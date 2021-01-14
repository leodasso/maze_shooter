using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class Gate : MonoBehaviour
{
    [ToggleLeft, SerializeField, ShowInInspector, Tooltip("Is this gate traversable? "), TabGroup("Main")]
    bool activated = true;
    
    [TabGroup("Main")]
    public Stage destination;
    
    [Tooltip("GateLinks are shared between pairs of gates. This lets the game know which gate in the destination stage" +
             " the player will start at."), TabGroup("Main")]
    public GateLink gateLink;
    
    [TabGroup("Main")]
    public Vector3 gateForward = Vector3.forward;

    [TabGroup("Events")]
    public UnityEvent onPlayerEnterGate;
    
    [TabGroup("Events")]
    public UnityEvent onPlayerSpawnToThisGate;

    [TabGroup("Events")]
    public UnityEvent onGateActivated;

    [TabGroup("Events")]
    public UnityEvent onGateDeactivated;

    public void ActivateGate()
    {
        activated = true;
        onGateActivated.Invoke();
    }

    public void DeactivateGate()
    {
        activated = false;
        onGateDeactivated.Invoke();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.rotation * gateForward * 5);
    }

    void Start()
    {
        // behave correctly if the 'activated' toggle is turned off in editor
        if (!activated) onGateDeactivated.Invoke();
		else onGateActivated.Invoke();
    }

    void SpawnPlayer()
    {
        onPlayerSpawnToThisGate.Invoke();
    }


    public void TryTransportPlayer()
    {
        if (!enabled || !activated) return;
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
