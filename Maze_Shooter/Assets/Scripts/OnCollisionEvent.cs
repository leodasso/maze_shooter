﻿using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using UnityEngine.Events;
using  Sirenix.OdinInspector;

[TypeInfoBox("Can call an event when this object collides with other 2D objects. Optionally can instantiate" + 
             " an effect at the collision point.")]
public class OnCollisionEvent : EffectsBase
{
    [ToggleLeft, Tooltip("Spawn an effect at the collision point? If not, you can ignore " +
                         " the effectPrefab, Lifetime, and Delay properties."), PropertyOrder(-999)]
    public bool spawnEffect;
    
    [Tooltip("For collisions within this velocity range, the events will trigger"), MinMaxSlider(0, 250, true)]
    public Vector2 velocityRange = new Vector2(0, 25);
    
    [Tooltip("Collisions with objects of these layers will cause the events to trigger. Any collisions with objects" +
            " outside of this layermask will be ignored by this component.")]
    public LayerMask triggeringLayers;
    
    public UnityEvent onCollision;

    void OnCollisionEnter(Collision other)
    {
        if (!Math.LayerMaskContainsLayer(triggeringLayers, other.collider.gameObject.layer)) return;

        float vel = other.relativeVelocity.magnitude;
        Debug.Log("Collisions velocity was " + vel);
        if (vel < velocityRange.x || vel > velocityRange.y) return;

        if (spawnEffect)
            InstantiateEffect(other.contacts[0].point);
        
        onCollision.Invoke();
    }
}