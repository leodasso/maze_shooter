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
	[ToggleLeft, PropertyOrder(-500)]
	public bool debug;
	[Tooltip("Layers that will destroy this object. Any inheriting class's behavior will happen before this is destroyed.")]
	public LayerMask layersThatDestroyThis;

	[Tooltip("Is this hazard expected to move at high velocity? If so, does a raycast check to make sure it doesn't go through" +
	         " any colliders.")]
	[PropertyOrder(-100)]
	public bool highVelocity = false;

	[ShowIf("highVelocity"), PropertyOrder(-99)]
	[Tooltip("Layers that will be checked against for the high velocity raycasting.")]
	public LayerMask castingLayerMask;
	public List<Collider> ignoredColliders = new List<Collider>();
	protected Vector3 _prevPosition;

	void Awake()
	{
		_prevPosition = transform.position;
		ignoredColliders.AddRange(GetComponentsInChildren<Collider>());
	}

	void Update()
	{
		if (!highVelocity) return;
		
		// Raycast from previous to current position
		Vector3 direction = transform.position - _prevPosition;
		Ray castingRay = new Ray(_prevPosition, direction);
		RaycastHit hit;
		if (Physics.Raycast(castingRay, out hit, direction.magnitude))
		if ( hit.collider != null && !ignoredColliders.Contains(hit.collider) && !hit.collider.isTrigger)
		{
			if (debug)
			{
				Debug.Log(name + " casted against " + hit.collider.name, gameObject);
			}
			transform.position = hit.point;
			Triggered(hit.collider);
		}
		// Reset previous position for next frame
		_prevPosition = transform.position;
	}

	void OnCollisionEnter(Collision other)
	{
		Collider otherCol = other.GetContact(0).otherCollider;
		
		// Don't collide with myself
		if (otherCol.gameObject == gameObject) return;
		
		// don't collide with ignored colliders
		if (ignoredColliders.Contains(otherCol)) return;
		        
		OnCollisionAction(other, otherCol);
		
		if (Math.LayerMaskContainsLayer(layersThatDestroyThis, otherCol.gameObject.layer)) 
			Destroy(gameObject);
	}

	void OnTriggerEnter(Collider other)
	{
		Triggered(other);
	}

	void Triggered(Collider other)
	{
		// don't collide with ignored colliders
		if (ignoredColliders.Contains(other)) return;
		
		OnTriggerAction(other);

		if (Math.LayerMaskContainsLayer(layersThatDestroyThis, other.gameObject.layer)) 
			Destroy(gameObject);
	}

	protected virtual void OnCollisionAction(Collision collision, Collider otherCol) {}
	protected virtual void OnTriggerAction(Collider other) {}
}
