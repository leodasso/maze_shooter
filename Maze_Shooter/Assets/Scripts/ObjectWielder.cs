﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using PlayMaker;
using ShootyGhost;

public class ObjectWielder : MonoBehaviour
{
	[SerializeField, Tooltip("Local position of an object while i wield it.")]
	Vector3 wieldedObjectOffset = Vector3.up;

	[Tooltip("Object to wield on init - calls PickUp() on start")]
	public GameObject wieldedObjectOnInit;

	FlingSword wieldedObject;

	[Tooltip("Calls event 'fling' ")]
	PlayMakerFSM wieldedObjectFSM;

	[SerializeField]
	[Tooltip("the rubber band ghost on me")]
	RubberBand ghostOnMe;

	[SerializeField]
	TargetFinder weaponFinder;

	[SerializeField, Tooltip("Within this range, i will pick up a weapon.")]
	float pickupDistance = 1;

	[SerializeField]
	UnityEvent onWeaponPickedUp;

	void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere(transform.position, pickupDistance);

		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.TransformPoint(wieldedObjectOffset), .25f);
	}

	void Start()
	{
		if (wieldedObjectOnInit)
			PickUp(wieldedObjectOnInit);
	}

	public void FlingMyObject()
	{
		// Disable the flingSword behavior so it doesn't get fling direction from its own rubber band
		wieldedObject.enabled = false;
		wieldedObject.gameObject.SetActive(true);
		wieldedObject.transform.parent = null;
		wieldedObject.flingDirection = ghostOnMe ? ghostOnMe.forceVector : Vector3.right;

		wieldedObjectFSM.SendEvent("fling");

		wieldedObject = null;
	}

	void PickUp(GameObject newWeapon)
	{
		FlingSword newWeaponSword = newWeapon.GetComponent<FlingSword>();
		if (!newWeaponSword) {
			Debug.LogError(name + " tried to pickup weapon but it has no fling sword component.", newWeapon);
			return;
		}

		Hauntable hauntable = newWeaponSword.GetComponent<Hauntable>();
		if (hauntable) 
			hauntable.EndHaunt();

		wieldedObject = newWeaponSword;
		wieldedObject.gameObject.SetActive(false);
		wieldedObject.transform.parent = transform;
		wieldedObject.transform.localPosition = wieldedObjectOffset;
		wieldedObjectFSM = wieldedObject.GetComponent<PlayMakerFSM>();

		onWeaponPickedUp.Invoke();

		Debug.Log("Picked up " + newWeapon.name);
	}


    // Update is called once per frame
    void Update()
    {
		if (wieldedObject) return;
		if (!weaponFinder) return;

		if (weaponFinder.currentTarget) {

			if (Vector3.Distance(transform.position, weaponFinder.currentTarget.transform.position) < pickupDistance) 
				PickUp(weaponFinder.currentTarget);
		}
    }
}