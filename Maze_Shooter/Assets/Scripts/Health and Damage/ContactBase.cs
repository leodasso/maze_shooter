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

	protected PseudoDepth _pseudoDepth;

	void Awake()
	{
		_pseudoDepth = GetComponent<PseudoDepth>();
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		Collider2D otherCol = other.GetContact(0).collider;
		
		// Don't collide with myself
		if (otherCol.gameObject == gameObject) return;

		if (!DepthsOverlap(otherCol)) return;
        
		OnCollisionAction(other, otherCol);
		
		if (Math.LayerMaskContainsLayer(layersThatDestroyThis, otherCol.gameObject.layer)) 
			Destroy(gameObject);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (!DepthsOverlap(other)) return;
		
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
