using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Crescent : MonoBehaviour
{
	public Orbiter myOrbiter;
	public OrbiterGroup orbiterGroup;
	public UnityEvent onCollected;

	public void Collect() {
		onCollected.Invoke();
		if (orbiterGroup) orbiterGroup.AddOrbiter(myOrbiter);
	}
}
