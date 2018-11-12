using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arachnid;
using Sirenix.OdinInspector;
using System.Linq;

[TypeInfoBox("Finds a target from the selected collection based on the given logic. Used by other components" +
             " which need a target specified.")]
public class TargetFinder : MonoBehaviour 
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
