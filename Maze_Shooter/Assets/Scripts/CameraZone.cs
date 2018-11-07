using System.Collections;
using System.Collections.Generic;
using Arachnid;
using Cinemachine;
using UnityEngine;

public class CameraZone : MonoBehaviour
{
	public CinemachineVirtualCamera vCam;
	[Tooltip("An object that belongs to any of these collections will trigger this")]
	public List<Collection> triggeringCollections;
	
	// Use this for initialization
	void Start () 
	{
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		CollectionElement element = other.GetComponent<CollectionElement>();
		if (!element) return;
		
		if (!triggeringCollections.Contains(element.collection)) return;
		EnterAction(other);
	}


	void EnterAction(Collider2D other)
	{
		vCam.Follow = other.transform;
	}

}
