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
	[Tooltip("Searches for targets in radius every .5 seconds. You can manually search by calling FindTarget()"), ToggleLeft]
	public bool autoAcquire = true;
	
	[Space]
	public Collection targets;
	
	public TargetType targetToAimAt;

	public float maxAqcuireRange = 100;

	public GameObject currentTarget;

	[SerializeField]
	UnityEvent onTargetFound;
	
	List<GameObject> targetsInRange = new List<GameObject>();

	void OnDrawGizmosSelected()
	{
		Gizmos.color = new Color(1, 1, .3f);
		Gizmos.DrawWireSphere(transform.position, maxAqcuireRange);
	}

	void OnDrawGizmos()
	{
		Gizmos.color = new Color(1, 1, .3f, .15f);
		Gizmos.DrawWireSphere(transform.position, maxAqcuireRange);
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

	public void FindTarget()
	{
		if (targets == null || targets.elements.Count < 1) return;
		
		targetsInRange.Clear();
		foreach (var target in targets.elements)
		{
			if (!target.gameObject.activeInHierarchy) continue;
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

	public void ClearTarget() 
	{
		currentTarget = null;
	}

	public void SetTarget(GameObject newTarget)
	{
		onTargetFound.Invoke();
		Debug.Log("Setting new target", newTarget);
		currentTarget = newTarget;
	}
}
