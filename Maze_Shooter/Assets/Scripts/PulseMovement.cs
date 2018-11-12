using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(TargetFinder))]
public class PulseMovement : MonoBehaviour
{
	public FloatReference speedMultiplier;
	public CurveObject speedCurve;
	float _totalSpeed;

	TargetFinder _targetFinder;

	// Use this for initialization
	void Start ()
	{
		_targetFinder = GetComponent<TargetFinder>();
		if (!speedCurve)
		{
			Debug.LogWarning(name + " has no speed curve defined, and it just don't work like that.");
			enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		_totalSpeed = speedMultiplier.Value * speedCurve.ValueFor(Time.time);
		if (!_targetFinder) return;
		if (!_targetFinder.currentTarget) return;
		
	}
}
