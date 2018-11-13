using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

[RequireComponent(typeof(TargetFinder))]
public class PulseMovement : MonoBehaviour
{
	public FloatReference startDelay;
	public FloatReference speedMultiplier;
	public CurveObject speedCurve;

	[DrawWithUnity]
	public UnityEvent onPulse;
	
	float _totalSpeed;
	TargetFinder _targetFinder;
	Rigidbody2D _rigidbody2D;
	float _delayTimer;
	
	//curve time value starts high so that it will call the 'onPulse' event immediately
	float _curveTime = 999;

	// Use this for initialization
	void Start ()
	{
		_targetFinder = GetComponent<TargetFinder>();
		_rigidbody2D = GetComponent<Rigidbody2D>();
		_delayTimer = startDelay.Value;
		if (!speedCurve)
		{
			Debug.LogWarning(name + " has no speed curve defined, and it just don't work like that.");
			enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (_delayTimer > 0)
		{
			_delayTimer -= Time.deltaTime;
			return;
		}

		_curveTime += Time.deltaTime;
		if (_curveTime >= speedCurve.curve.Duration())
		{
			onPulse.Invoke();
			_curveTime = 0;
		}
		_totalSpeed = speedMultiplier.Value * speedCurve.ValueFor(_curveTime);
	}

	void FixedUpdate()
	{
		if (!_targetFinder) return;
		if (!_targetFinder.currentTarget) return;

		Vector2 dir = _targetFinder.currentTarget.position - transform.position;
		_rigidbody2D.AddForce(dir.normalized * _totalSpeed);
	}
}
