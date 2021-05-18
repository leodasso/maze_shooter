using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using PlayMaker;

public class ObjectWielder : MonoBehaviour
{
	[SerializeField]
	[Tooltip("Sets the fling direction of this object")]
	FlingSword wieldedObject;

	[SerializeField]
	[Tooltip("Calls event 'fling' ")]
	PlayMakerFSM wieldedObjectFSM;

	[SerializeField]
	[Tooltip("the rubber band ghost on me")]
	RubberBand ghostOnMe;

	public void FlingMyObject()
	{
		// Disable the flingSword behavior so it doesn't get fling direction from its own rubber band
		wieldedObject.enabled = false;
		wieldedObject.gameObject.SetActive(true);
		wieldedObject.transform.parent = null;
		wieldedObject.flingDirection = ghostOnMe ? ghostOnMe.forceVector : Vector3.right;

		wieldedObjectFSM.SendEvent("fling");
	}

	public void PickUp()
	{
		// TODO
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
