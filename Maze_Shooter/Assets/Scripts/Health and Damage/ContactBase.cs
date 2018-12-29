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
	public LayerMask layersToDamage;

	[ToggleLeft, Tooltip("Destroy this gameObject after colliding with an object. Damages object first")]
	public bool destroyOnHit;

	void OnCollisionEnter2D(Collision2D other)
	{
		Collider2D otherCol = other.GetContact(0).collider;
		if (!Math.LayerMaskContainsLayer(layersToDamage, otherCol.gameObject.layer)) 
			return;
        
		OnCollisionAction(other, otherCol);
		
		if (destroyOnHit) 
			Destroy(gameObject);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (!Math.LayerMaskContainsLayer(layersToDamage, other.gameObject.layer)) 
			return;
        
		OnTriggerAction(other);
		
		if (destroyOnHit) 
			Destroy(gameObject);
	}

	protected virtual void OnCollisionAction(Collision2D collision, Collider2D otherCol) {}
	protected virtual void OnTriggerAction(Collider2D other) {}
}
