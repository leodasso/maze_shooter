using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using Sirenix.OdinInspector;

[TypeInfoBox("Add this component to any object with a 2D collider, and it will deal damage to other iDestructible objects" +
             "that it comes into contact with.")]
public class Hazard : ContactBase
{
    [Tooltip("Layers that this hazard will do damage to")]
    public LayerMask layersToDamage;
	public IntReference damage;

    protected override void OnCollisionAction(Collision2D collision, Collider2D otherCol)
    {
        if (!Math.LayerMaskContainsLayer(layersToDamage, otherCol.gameObject.layer)) return;
        
        IDestructible destructible = otherCol.GetComponent<IDestructible>();
        destructible?.DoDamage(damage.Value, collision.GetContact(0).point, collision.GetContact(0).normal);
    }

    protected override void OnTriggerAction(Collider2D other)
    {
        if (!Math.LayerMaskContainsLayer(layersToDamage, other.gameObject.layer)) return;
        
        IDestructible destructible = other.GetComponent<IDestructible>();
        destructible?.DoDamage(damage.Value, ((Vector2)transform.position + (Vector2)other.transform.position)/2, 
            (transform.position - other.transform.position).normalized);
    }
}
