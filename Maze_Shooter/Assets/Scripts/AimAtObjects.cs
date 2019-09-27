using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Arachnid;
using Sirenix.OdinInspector;

[RequireComponent(typeof(TargetFinder))]
public class AimAtObjects : MonoBehaviour
{
	TargetFinder _targetFinder;
	
	// Use this for initialization
	void Start ()
	{
		_targetFinder = GetComponent<TargetFinder>();
	}

	// Update is called once per frame
	void Update ()
	{
		if (_targetFinder == null) return;
		if (_targetFinder.currentTarget == null) return;

		float z = Math.AngleFromVector2(_targetFinder.currentTarget.transform.position - transform.position, -90);
		transform.eulerAngles = new Vector3(0, 0, z);
	}
}
