using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

[RequireComponent(typeof(TargetFinder))]
public class PulseMovement : MonoBehaviour
{
	[TabGroup("main")]
	public FloatReference startDelay;
	[Tooltip("A random amount of this value will be added to start delay"), TabGroup("main")]
	public FloatReference randomStartDelay;
	
	[Tooltip("Multiplies the movement speed"), TabGroup("main")]
	public FloatReference speedMultiplier;

	[TabGroup("main")]
	[Tooltip("Multiplies the rate at which the curve is evaluated. Something like 'doing the pulse more times per minte'")]
	public float timeMultiplier = 1;
	
	[TabGroup("main")]
	public CurveObject speedCurve;

	[TabGroup("Events")]
	[DrawWithUnity, Tooltip("A pulse is a run through the speed curve. This event will be called at the beginning of each pulse.")]
	public UnityEvent onPulse;
	
	[TabGroup("Events"), Tooltip("Use this to call an event anywhere along the pulse timeline.")]
	public List<PulseEvent> pulseEvents = new List<PulseEvent>();
	
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
		_delayTimer = startDelay.Value + Random.Range(0, randomStartDelay.Value);
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

		_curveTime += Time.deltaTime * timeMultiplier;
		if (_curveTime >= speedCurve.curve.Duration())
		{
			onPulse.Invoke();
			_curveTime = 0;
			foreach (var e in pulseEvents)
				e.Reset();
		}

		float normalizedProgress = _curveTime / speedCurve.curve.Duration();
		foreach (var e in pulseEvents)
			e.TryEvent(normalizedProgress);
		
		_totalSpeed = speedMultiplier.Value * speedCurve.ValueFor(_curveTime) * timeMultiplier;
	}

	void FixedUpdate()
	{
		if (!_targetFinder) return;
		if (!_targetFinder.currentTarget) return;

		Vector2 dir = _targetFinder.currentTarget.position - transform.position;
		_rigidbody2D.AddForce(dir.normalized * _totalSpeed);
	}

	public void IncreaseTimeMultiplier(float amt)
	{
		timeMultiplier += amt;
	}
	
	[System.Serializable]
	public class PulseEvent
	{
		[Range(0, 1), Tooltip("Normalized time (0 is beginning, 1 is end) representing when to call this event.")]
		public float eventTriggerTime = .5f;
		[DrawWithUnity]
		public UnityEvent pulseEvent;
		bool _triggered;
		
		public void TryEvent(float time)
		{
			if (_triggered) return;
			if (time < eventTriggerTime) return;
			_triggered = true;
			pulseEvent.Invoke();
		}

		public void Reset()
		{
			_triggered = false;
		}
	}
}
