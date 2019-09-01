using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Arachnid;

/// <summary>
/// Base class for interactions on trigger or collision between 2D elements
/// </summary>
public class ContactBase : MonoBehaviour 
{
	[Tooltip("Layers that will destroy this object. Any inheriting class's behavior will happen before this is destroyed.")]
	public LayerMask layersThatDestroyThis;

	void OnCollisionEnter2D(Collision2D other)
	{
		Collider2D otherCol = other.GetContact(0).collider;
		
		// Don't collide with myself
		if (otherCol.gameObject == gameObject) return;
        
		OnCollisionAction(other, otherCol);
		
		if (Math.LayerMaskContainsLayer(layersThatDestroyThis, otherCol.gameObject.layer)) 
			Destroy(gameObject);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		OnTriggerAction(other);
		
		if (Math.LayerMaskContainsLayer(layersThatDestroyThis, other.gameObject.layer)) 
			Destroy(gameObject);
	}

	protected virtual void OnCollisionAction(Collision2D collision, Collider2D otherCol) {}
	protected virtual void OnTriggerAction(Collider2D other) {}
}
