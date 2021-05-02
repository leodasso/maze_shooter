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

	[ShowInInspector, ReadOnly]
	bool camIsActive;

	List<Collider> targetsInZone = new List<Collider>();

	int initPriority;

	bool ZoneOccupied() 
	{
		foreach (var target in targetsInZone) 
		{
			if (target != null && target.gameObject.activeInHierarchy && target.enabled) 
				return true;
		}
		return false;
	}

	void Awake() 
	{
		initPriority = vCam.Priority;
	}

	void Update() 
	{
		// Unity doesn't call OnTriggerExit if a trigger is just deactivated,
		// so we need to manually check if that's the case
		if (camIsActive && !ZoneOccupied())
			ExitAction(null);
	}
	
	
	void OnTriggerEnter(Collider other)
	{
		if (camIsActive) return;

		CollectionElement element = other.GetComponent<CollectionElement>();
		if (!element) return;
		
		if (!triggeringCollections.Contains(element.collection)) return;
		EnterAction(other);
	}

	void OnTriggerExit(Collider other)
	{
		if (!camIsActive) return;

		CollectionElement element = other.GetComponent<CollectionElement>();
		if (!element) return;
		
		if (!triggeringCollections.Contains(element.collection)) return;
		ExitAction(other);
	}


	void EnterAction(Collider other)
	{
		targetsInZone.Add(other);
		camIsActive = true;
		if (overwriteFollowTarget) 
			vCam.Follow = other.transform;

		if (adjustPriority) 
			vCam.Priority = activeCamPriority;
	}

	void ExitAction(Collider other)
	{
		targetsInZone.Remove(other);
		camIsActive = false;
		if (adjustPriority) 
			vCam.Priority = initPriority;
	}

}
