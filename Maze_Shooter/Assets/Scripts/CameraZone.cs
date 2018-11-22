﻿using System.Collections;
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

	[ShowIf("adjustPriority")]
	public int priorityDelta = 1;
	
	
	void OnTriggerEnter2D(Collider2D other)
	{
		CollectionElement element = other.GetComponent<CollectionElement>();
		if (!element) return;
		
		if (!triggeringCollections.Contains(element.collection)) return;
		EnterAction(other);
	}

	void OnTriggerExit2D(Collider2D other)
	{
		CollectionElement element = other.GetComponent<CollectionElement>();
		if (!element) return;
		
		if (!triggeringCollections.Contains(element.collection)) return;
		ExitAction(other);
	}


	void EnterAction(Collider2D other)
	{
		vCam.Follow = other.transform;
		if (adjustPriority) vCam.Priority += priorityDelta;
	}

	void ExitAction(Collider2D other)
	{
		if (adjustPriority) vCam.Priority -= priorityDelta;
	}

}
