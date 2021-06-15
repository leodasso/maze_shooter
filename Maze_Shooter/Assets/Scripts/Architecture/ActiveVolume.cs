using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ActiveVolume : MonoBehaviour
{
	public float radius;

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(transform.position, radius);
	}

	void Update()
	{
		foreach ( var actor in Actor.allActors) 
		{
			bool isInRange = Arachnid.Math.IsInRange(transform.position - actor.transform.position, radius);

			if (actor.culled && isInRange)
				actor.Activate();

			else if (!actor.culled && !isInRange)
				actor.Deactivate();
		}
	}
}