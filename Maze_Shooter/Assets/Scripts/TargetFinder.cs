using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arachnid;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine.Events;

[TypeInfoBox("Finds a target from the selected collection based on the given logic. Used by other components" +
             " which need a target specified.")]
public class TargetFinder : MonoBehaviour
{
	public bool autoAcquire = true;
	
	[ShowIf("autoAcquire")]
	public Collection targets;
	
	[ShowIf("autoAcquire")]
	public TargetType targetToAimAt;

	[ShowIf("autoAcquire")]
	public float maxAqcuireRange = 100;

	public GameObject currentTarget;

	public UnityEvent onTargetFound;
	
	List<GameObject> targetsInRange = new List<GameObject>();

	void OnDrawGizmosSelected()
	{
		if (autoAcquire)
		{
			Gizmos.color = new Color(1, 1, .3f);
			Gizmos.DrawWireSphere(transform.position, maxAqcuireRange);
		}
	}

	void OnDrawGizmos()
	{
		if (autoAcquire)
		{
			Gizmos.color = new Color(1, 1, .3f, .15f);
			Gizmos.DrawWireSphere(transform.position, maxAqcuireRange);
		}
	}

	// Use this for initialization
	void Start () 
	{
		FindTarget();
	}
	
	void OnEnable()
	{
		InvokeRepeating(nameof(TryFindTarget), 0, .5f);
	}
	
	void TryFindTarget()
	{
		if (currentTarget != null || !autoAcquire) return;
		FindTarget();
	}

	void FindTarget()
	{
		if (targets == null || !autoAcquire) return;
		if (targets.elements.Count < 1) return;
		
		targetsInRange.Clear();
		foreach (var target in targets.elements)
		{
			if (Vector3.SqrMagnitude(target.transform.position - transform.position) < maxAqcuireRange * maxAqcuireRange)
				targetsInRange.Add(target.gameObject);
		}
		
		if (targetsInRange.Count < 1) return;

		if (targetToAimAt == TargetType.Random)
		{
			SetTarget(Arachnid.Math.RandomElementOfList(targetsInRange));
			return;
		}

		// Order targets list by distance to this
		targetsInRange = targetsInRange.OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).ToList();
		
		if (targetToAimAt == TargetType.Farthest)
		{
			SetTarget(targetsInRange.Last());
			return;
		}

		if (targetToAimAt == TargetType.Nearest)
			SetTarget(targetsInRange.First());
	}

	public void SetTarget(GameObject newTarget)
	{
		onTargetFound.Invoke();
		Debug.Log("Setting new target", newTarget);
		currentTarget = newTarget;
	}
}
