using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ActiveVolume : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        ActorPlaceholder otherActor = other.GetComponent<ActorPlaceholder>();
        if (!otherActor) return;
        otherActor.Activate();
    }

    void OnTriggerExit(Collider other)
    {
        Actor otherActor = other.GetComponent<Actor>();
        if (!otherActor) return;
        otherActor.Deactivate();
    }
}
