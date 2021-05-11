using System.Collections;
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

	float radius;

	Vector3 forceVector;
	Vector2 _input;

    // Start is called before the first frame update
    void Start()
    {
        
    }

	void Update()
	{
		// grow the radius as player pulls
		radius += _input.magnitude * Time.deltaTime * radiusGrowthRate.Evaluate(radius);
		radius = Mathf.Clamp(radius, 0, maxRadius);

		bool playerPulling = _input.magnitude > .05f;

		if (!playerPulling) 
			radius = 0.001f;

		var linearLimitSpring = joint.linearLimitSpring;
		linearLimitSpring.spring = playerPulling ? 0 : springForce;
		joint.linearLimitSpring = linearLimitSpring;

		var linearLimit = joint.linearLimit;
		linearLimit.limit = radius;
		joint.linearLimit = linearLimit;
	}

	void FixedUpdate() 
	{
		forceVector = Arachnid.Math.Project2Dto3D(_input);
		rubberBandRigidBody.AddForce(forceVector * inputStrength * Time.fixedDeltaTime * 1000);
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
