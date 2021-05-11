﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[AddComponentMenu("Character Controllers/Rubber Band")]
public class RubberBand : MonoBehaviour, IControllable
{
	public Rigidbody rubberBandRigidBody;
	public ConfigurableJoint joint;

	public float maxRadius = 3;

	[Tooltip("Units per second the radius grows as player holds down joystick \n \n" + 
	"x axis is radius, y axis is force applied when at that radius.")]
	public AnimationCurve radiusGrowthRate = AnimationCurve.Linear(0, 5, 3, .1f);

	[Tooltip("The spring force applied when the player lets go of joystick")]
	public float springForce = 100;

	[Tooltip("The amount of force that will be applied to the rubber band rigidbody.")]
	public float inputStrength = 1;

	[Tooltip("When activating, how wobbly should this be?")]
	public float initWobbliness = .5f;

	[Space]
	[Tooltip("Drag while pulling")]
	public float pullingDrag;
	public float normalDrag;

	public float NormalizedRadius => radius / maxRadius;
	public Vector3 FlingDirection => forceVector;

	float radius;

	Vector3 forceVector;
	Vector2 _input;
	Vector3 jointLocalInitPos;

    // Start is called before the first frame update
    void Start()
    {
		jointLocalInitPos = joint.transform.localPosition;
		ResetJoint();
    }

	void Update()
	{
		// grow the radius as player pulls
		radius += _input.magnitude * Time.deltaTime * radiusGrowthRate.Evaluate(radius);
		radius = Mathf.Clamp(radius, 0, maxRadius);

		bool playerPulling = _input.magnitude > .05f;

		rubberBandRigidBody.drag = playerPulling ? pullingDrag : normalDrag;

		if (!playerPulling) 
			radius = 0.001f;

		var linearLimitSpring = joint.linearLimitSpring;
		linearLimitSpring.spring = playerPulling ? 0 : springForce;
		joint.linearLimitSpring = linearLimitSpring;

		var linearLimit = joint.linearLimit;
		linearLimit.limit = radius;
		joint.linearLimit = linearLimit;
	}

	public void ResetJoint() 
	{
        joint.connectedAnchor = transform.TransformPoint(jointLocalInitPos);
		joint.transform.localPosition = jointLocalInitPos + Random.onUnitSphere * initWobbliness;
	}
	

	void FixedUpdate() 
	{
		forceVector = Arachnid.Math.Project2Dto3D(_input);
		rubberBandRigidBody.AddForce(forceVector * inputStrength * Time.fixedDeltaTime * 1000);
	}

	public void OnPlayerControlEnabled(bool isEnabled)
	{
		Debug.Log("Rubber band player control change. enabled: " + isEnabled);
		if (!isEnabled)
			_input = Vector2.zero;
	}

	public void ApplyLeftStickInput(Vector2 input) 
	{
		_input = input;
	}

	public void ApplyRightStickInput(Vector2 input) 
	{
	}

	public void DoActionAlpha() 
	{
	}

	public string Name() 
	{
		return "Rubber Band " + name;
	}
}
