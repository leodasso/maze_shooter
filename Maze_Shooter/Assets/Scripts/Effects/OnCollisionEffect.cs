using Arachnid;
using UnityEngine;

public class OnCollisionEffect : EffectsBase
{
    public FloatReference minCollisionVelocity;
    [Tooltip("Only collisions with objects in these layers will trigger an effect")]
    public LayerMask layerMask;
    
    void OnCollisionEnter(Collision other)
    {
        if (other.relativeVelocity.magnitude < minCollisionVelocity.Value)
            return;
        
        if (!Math.LayerMaskContainsLayer(layerMask, other.gameObject.layer))
            return;
        
        InstantiateEffect(other.contacts[0].point);
    }
}
