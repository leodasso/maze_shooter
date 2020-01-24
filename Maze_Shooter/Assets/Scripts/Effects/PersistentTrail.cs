using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[TypeInfoBox("When this object is disabled or destroyed, moves any effects or trails that need to be " +
             "persistent to the root effects parent.")]
public class PersistentTrail : MonoBehaviour
{
    [Tooltip("How long will the effects persist after this object is destroyed or disabled")]
    public float effectsPersistTime = 4;
    [Tooltip("Stop emission of particles when they're moved from this object.")]
    public bool stopParticlesOnDeparent;
    
    public List<GameObject> persistentEffects = new List<GameObject>();

    void OnDisable()
    {
        foreach (var go in persistentEffects)
        {
            go.transform.parent = EffectsBase.EffectsParent().transform;
            if (stopParticlesOnDeparent)
            {
                ParticleSystem particleSystem = go.GetComponent<ParticleSystem>();
                if (particleSystem)
                    particleSystem.Stop();
            }
            Destroy(go, effectsPersistTime);
        }
    }
}
