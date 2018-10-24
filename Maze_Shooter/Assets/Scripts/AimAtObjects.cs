using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Arachnid;
using Sirenix.OdinInspector;

public class AimAtObjects : MonoBehaviour 
{

	public Collection targets;
	public TargetType targetToAimAt;

	public Transform currentTarget;

	// Use this for initialization
	void Start () 
	{
		FindTarget();
	}

	void OnEnable()
	{
		InvokeRepeating(nameof(TryFindTarget), 0, .5f);
	}

	// Update is called once per frame
	void Update ()
	{
		if (currentTarget == null) return;

		float z = Math.AngleFromVector2(currentTarget.position - transform.position, -90);
		transform.eulerAngles = new Vector3(0, 0, z);
	}

	void TryFindTarget()
	{
		if (currentTarget != null) return;
		FindTarget();
	}

	void FindTarget()
	{
		if (targets == null) return;
		if (targets.elements.Count < 1) return;

		if (targetToAimAt == TargetType.Random)
		{
			CollectionElement randomTarget;
			if (targets.GetRandom(out randomTarget))
			{
				currentTarget = randomTarget.transform;
				return;
			}
		}

		List<Transform> orderedTargets = targets.GetElementsOfType<Transform>();
		orderedTargets = orderedTargets.OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).ToList();
		
		if (targetToAimAt == TargetType.Farthest)
		{
			currentTarget = orderedTargets.Last();
			return;
		}

		if (targetToAimAt == TargetType.Nearest)
			currentTarget = orderedTargets.First();
	}
}
