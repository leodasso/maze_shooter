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

	[ToggleLeft]
	public bool adjustPriority;

	[ToggleLeft, Tooltip("Set whatever triggers this as the follow target on the virtual camera.")]
	public bool overwriteFollowTarget = true;

	[ShowIf("adjustPriority")]
	public int priorityDelta = 1;
	
	
	void OnTriggerEnter(Collider other)
	{
		CollectionElement element = other.GetComponent<CollectionElement>();
		if (!element) return;
		
		if (!triggeringCollections.Contains(element.collection)) return;
		EnterAction(other);
	}

	void OnTriggerExit(Collider other)
	{
		CollectionElement element = other.GetComponent<CollectionElement>();
		if (!element) return;
		
		if (!triggeringCollections.Contains(element.collection)) return;
		ExitAction(other);
	}


	void EnterAction(Collider other)
	{
		if (overwriteFollowTarget) vCam.Follow = other.transform;
		if (adjustPriority) vCam.Priority += priorityDelta;
	}

	void ExitAction(Collider other)
	{
		if (adjustPriority) vCam.Priority -= priorityDelta;
	}

}
