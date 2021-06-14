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
	[BoxGroup("contact")]
	[ToggleLeft, PropertyOrder(-500)]
	public bool debug;

	[BoxGroup("contact")]
	[ToggleLeft, PropertyOrder(-500)]
	public bool ignoreTriggers = true;
	
	[BoxGroup("contact")]
	[Tooltip("Layers that will destroy this object. Any inheriting class's behavior will happen before this is destroyed.")]
	public LayerMask layersThatDestroyThis;

	[BoxGroup("contact")]
	[Tooltip("Is this hazard expected to move at high velocity? If so, does a raycast check to make sure it doesn't go through any colliders.")]
	[PropertyOrder(-100), ToggleLeft]
	public bool highVelocity = false;

	[BoxGroup("contact")]
	[ShowIf("highVelocity"), PropertyOrder(-99)]
	[Tooltip("Layers that will be checked against for the high velocity raycasting.")]
	public LayerMask castingLayerMask;
	
	[BoxGroup("contact")]
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
		if (Physics.Raycast(castingRay, out hit, direction.magnitude)) {
			if (CanHitCollider(hit.collider))
			{
				if (debug)
				{
					Debug.Log(name + " casted against " + hit.collider.name, gameObject);
				}
				transform.position = hit.point;
				Triggered(hit.collider);
			}
		}

		// Reset previous position for next frame
		_prevPosition = transform.position;
	}

	bool CanHitCollider(Collider other) 
	{
		if (!other) return false;
		if (ignoreTriggers && other.isTrigger) return false;
		if (ignoredColliders.Contains(other)) return false;
		if (other.gameObject == gameObject) return false;
		return true;
	}

	void OnCollisionEnter(Collision other)
	{
		Collider otherCol = other.GetContact(0).otherCollider;

		if (debug)
			Debug.Log(name + " collided with " + other.gameObject.name);

		if (!CanHitCollider(otherCol)) {
			if (debug)
				Debug.Log("    The collider " + otherCol.name + " is in the 'cant hit' list, so this is ignored.");
			return;
		}
		        
		OnCollisionAction(other, otherCol);
		
		if (Math.LayerMaskContainsLayer(layersThatDestroyThis, otherCol.gameObject.layer)) 
			Destroy(gameObject);
	}

	void OnTriggerEnter(Collider other)
	{
		if (CanHitCollider(other))
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
