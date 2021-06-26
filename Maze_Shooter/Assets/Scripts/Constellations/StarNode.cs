using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Sirenix.OdinInspector;

public class StarNode : MonoBehaviour
{
	[FormerlySerializedAs("linkedConstellation")]
    public StarData linkedStar;
    
	public UnityEvent onSlotFilled;

	public void Fill() {
		onSlotFilled.Invoke();
	}

	[Button]
	void NameMe() {
		gameObject.name = "Node " + linkedStar.title;
	}
}
