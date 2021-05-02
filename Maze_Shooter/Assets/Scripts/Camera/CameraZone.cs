using System.Collections;
using System.Collections.Generic;
using Arachnid;
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;

public class CameraZone : MonoBehaviour
{
	
	public CinemachineVirtualCamera vCam;
	[Tooltip("An object that belongs to any of these collections will trigger this")]
	public List<Collection> triggeringCollections;

	[Space, ToggleLeft, Tooltip("Set whatever triggers this as the follow target on the virtual camera.")]
	public bool overwriteFollowTarget = true;

	[ToggleLeft, Tooltip("Changes the priority of the vCam when a target is in this camera zone.")]
	public bool adjustPriority;

	[ShowIf("adjustPriority"), UnityEngine.Serialization.FormerlySerializedAs("priorityDelta")]
	[Tooltip("The priority this vCam is at when there's a target in this camera zone.")]
	public int activeCamPriority = 1;

	[Space, ShowInInspector, ReadOnly]
	bool targetInZone;

	int initPriority;

	void Awake() 
	{
		initPriority = vCam.Priority;
	}
	
	
	void OnTriggerEnter(Collider other)
	{
		if (targetInZone) return;

		CollectionElement element = other.GetComponent<CollectionElement>();
		if (!element) return;
		
		if (!triggeringCollections.Contains(element.collection)) return;
		EnterAction(other);
	}

	void OnTriggerExit(Collider other)
	{
		if (!targetInZone) return;

		CollectionElement element = other.GetComponent<CollectionElement>();
		if (!element) return;
		
		if (!triggeringCollections.Contains(element.collection)) return;
		ExitAction(other);
	}


	void EnterAction(Collider other)
	{
		targetInZone = true;
		if (overwriteFollowTarget) 
			vCam.Follow = other.transform;

		if (adjustPriority) 
			vCam.Priority = activeCamPriority;
	}

	void ExitAction(Collider other)
	{
		targetInZone = false;
		if (adjustPriority) 
			vCam.Priority = initPriority;
	}

}
