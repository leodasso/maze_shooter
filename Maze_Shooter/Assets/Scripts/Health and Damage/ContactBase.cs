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

	[Tooltip("Is this hazard expected to move at high velocity? If so, does a raycast check to make sure it doesn't go through" +
	         " any colliders.")]
	[PropertyOrder(-100)]
	public bool highVelocity = false;

	[ShowIf("highVelocity"), PropertyOrder(-99)]
	[Tooltip("Layers that will be checked against for the high velocity raycasting.")]
	public LayerMask castingLayerMask;
	
	public List<Collider2D> ignoredColliders = new List<Collider2D>();

	protected Vector3 _prevPosition;

	protected PseudoDepth _pseudoDepth;

	void Awake()
	{
		_pseudoDepth = GetComponent<PseudoDepth>();
		_prevPosition = transform.position;
	}

	void Update()
	{
		if (!highVelocity) return;
		
		// Raycast from previous to current position
		Vector3 direction = transform.position - _prevPosition;
		RaycastHit2D hit = Physics2D.Raycast(_prevPosition, direction.normalized, direction.magnitude, castingLayerMask);
		if ( hit.collider != null && !ignoredColliders.Contains(hit.collider))
		{
			transform.position = hit.point;
			Triggered(hit.collider);
		}
		// Reset previous position for next frame
		_prevPosition = transform.position;
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		Collider2D otherCol = other.GetContact(0).collider;
		
		// Don't collide with myself
		if (otherCol.gameObject == gameObject) return;
		
		// don't collide with ignored colliders
		if (ignoredColliders.Contains(otherCol)) return;
		
		if (!DepthsOverlap(otherCol)) return;
        
		OnCollisionAction(other, otherCol);
		
		if (Math.LayerMaskContainsLayer(layersThatDestroyThis, otherCol.gameObject.layer)) 
			Destroy(gameObject);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		Triggered(other);
	}

	void Triggered(Collider2D other)
	{
		if (!DepthsOverlap(other)) return;
		
		// don't collide with ignored colliders
		if (ignoredColliders.Contains(other)) return;
		
		OnTriggerAction(other);

		if (Math.LayerMaskContainsLayer(layersThatDestroyThis, other.gameObject.layer)) 
			Destroy(gameObject);
	}

	/// <summary>
	/// Returns whether or not this depth overlaps with the other collider's depth. If no pseudoDepth components
	/// are involved, this will always be true.
	/// </summary>
	bool DepthsOverlap(Collider2D other)
	{
		PseudoDepth otherPseudoDepth = other.gameObject.GetComponent<PseudoDepth>();
		if (_pseudoDepth && otherPseudoDepth)
			return _pseudoDepth.OverlapWith(otherPseudoDepth);

		if (_pseudoDepth && !otherPseudoDepth)
			return _pseudoDepth.DefaultOverlap();

		if (!_pseudoDepth && otherPseudoDepth)
			return otherPseudoDepth.DefaultOverlap();

		return true;
	}

	protected virtual void OnCollisionAction(Collision2D collision, Collider2D otherCol) {}
	protected virtual void OnTriggerAction(Collider2D other) {}
}
