using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class ConstellationNode : MonoBehaviour
{
    public ConstellationData linkedConstellation;
    
	public UnityEvent onSlotFilled;

	public void Fill() {
		onSlotFilled.Invoke();
	}

	[Button]
	void NameMe() {
		gameObject.name = "Node " + linkedConstellation.title;
	}
}
