using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// When actors deactivate, they need a trigger in their place so that if the active volume crosses over them again,
/// they can re-activate. This object acts as an inbetween, telling the actor when to re-activate.
/// </summary>
public class ActorPlaceholder : MonoBehaviour
{
    public Actor actor;

    public void Activate()
    {
        if (!actor) return;
        actor.Activate();
    }
}
