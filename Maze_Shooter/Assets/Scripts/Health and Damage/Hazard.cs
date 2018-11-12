using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using Sirenix.OdinInspector;

[TypeInfoBox("Add this component to any object with a 2D collider, and it will deal damage to other iDestructible objects" +
             "that it comes into contact with.")]
public class Hazard : MonoBehaviour
{
	public IntReference damage;
    public LayerMask layersToDamage;

    void OnCollisionEnter2D(Collision2D other)
    {
        Collider2D otherCol = other.GetContact(0).collider;
        if (!Math.LayerMaskContainsLayer(layersToDamage, otherCol.gameObject.layer)) 
            return;
        
        IDestructible destructible = otherCol.GetComponent<IDestructible>();
        destructible?.DoDamage(damage.Value, other.GetContact(0).point, other.GetContact(0).normal);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!Math.LayerMaskContainsLayer(layersToDamage, other.gameObject.layer)) 
            return;
        
        IDestructible destructible = other.GetComponent<IDestructible>();
        destructible?.DoDamage(damage.Value, ((Vector2)transform.position + (Vector2)other.transform.position)/2, 
            (transform.position - other.transform.position).normalized);
    }
}
