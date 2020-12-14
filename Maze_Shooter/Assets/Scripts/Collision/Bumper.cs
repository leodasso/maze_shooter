using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Arachnid;

public class Bumper : MonoBehaviour
{
	public float minVelocity = 1;
	public LayerMask layerMask;
	public UnityEvent onBump;

	void OnCollisionEnter(Collision other) 
	{
		Collider otherCol = other.GetContact(0).otherCollider;
		float relativeVel = other.relativeVelocity.magnitude;
		if (relativeVel < minVelocity) return;
		
		if (Math.LayerMaskContainsLayer(layerMask, otherCol.gameObject.layer)) {
			onBump.Invoke();
		}
	}


}
