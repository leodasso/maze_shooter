using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[AddComponentMenu("Character Controllers/Rubber Band")]
public class RubberBand : MonoBehaviour, IControllable
{
	[Tooltip("Transform where ghost will return when haunting complete. This is moved by this component")]
	public Transform hauntReturnPos;

	[Tooltip("distance that the ghost will be returned to when unhaunting.")]
	public float ghostHauntReturnDist = 6;

	[Space]
	public Rigidbody rubberBandRigidBody;
	[Tooltip("The object that connects to the ground ")]
	public Transform foot;
	public ConfigurableJoint joint;

	public float maxRadius = 3;

	[Tooltip("How far to translate the base from init local pos when pulling.")]
	public float footMovement = .1f;

	[Tooltip("Units per second the radius grows as player holds down joystick \n \n" + 
	"x axis is radius, y axis is force applied when at that radius.")]
	public AnimationCurve radiusGrowthRate = AnimationCurve.Linear(0, 5, 3, .1f);

	[Tooltip("The spring force applied when the player lets go of joystick")]
	public float springForce = 100;

	[Tooltip("The amount of force that will be applied to the rubber band rigidbody.")]
	public float inputStrength = 1;

	[Tooltip("When activating, how wobbly should this be?")]
	public float initWobbliness = .5f;

	[Range(0, 1)]
	[Tooltip("How hard the player needs to pull before flinging happens. \n 'flinging' can be anything - just link it up in the onFling event below")]
	public float flingThreshhold = .9f;

	[Space]
	[Tooltip("Drag while pulling")]
	public float pullingDrag;
	public float normalDrag;

	public float NormalizedRadius => radius / maxRadius;

	[SerializeField]
	UnityEvent onFling;

	float radius;

	Vector3 pullVector;
	public Vector3 forceVector;
	Vector2 _input;
	Vector3 jointLocalInitPos;
	Vector3 footLocalInitPos;

	bool canFling;


    // Start is called before the first frame update
    void Start()
    {
		footLocalInitPos = foot.transform.localPosition;
		jointLocalInitPos = joint.transform.localPosition;
		ResetJoint();

		canFling = true;
    }

	void Update()
	{
		// grow the radius as player pulls
		pullVector += (Vector3)_input * Time.deltaTime * radiusGrowthRate.Evaluate(radius);
		radius = Mathf.Clamp(pullVector.magnitude, 0, maxRadius);

		bool playerPulling = _input.magnitude > .05f;

		// move local pos from pulling
		if (foot) {
			if (playerPulling ) 
				foot.transform.localPosition = pullVector.normalized * footMovement + footLocalInitPos;
			else	
				foot.transform.localPosition = footLocalInitPos;
		}

		joint.connectedAnchor = transform.TransformPoint(jointLocalInitPos);

		rubberBandRigidBody.drag = playerPulling ? pullingDrag : normalDrag;

		// Reset radius when player lets go of stick
		if (!playerPulling) {
			pullVector = Vector3.zero;
			radius = 0.001f;
		}

		// enable springiness when player lets go for fun bouncy effect
		var linearLimitSpring = joint.linearLimitSpring;
		linearLimitSpring.spring = playerPulling ? 0 : springForce;
		joint.linearLimitSpring = linearLimitSpring;

		// Apply the stretch radius to the joint
		var linearLimit = joint.linearLimit;
		linearLimit.limit = radius;
		joint.linearLimit = linearLimit;

		if (radius < .01f && !canFling)
			AllowFling();


		if (NormalizedRadius >= flingThreshhold && canFling) 
			InvokeFling();



		if (hauntReturnPos) {
			Vector3 hauntReturn = forceVector * ghostHauntReturnDist;
			if (hauntReturn.magnitude < 3)
				hauntReturn = Vector3.right * 3;
			hauntReturnPos.position = transform.position + Vector3.Scale(hauntReturn, new Vector3(1, 0, 1));
		}
	}

	void InvokeFling()
	{
		canFling = false;
		onFling.Invoke();
	}

	public void AllowFling()
	{
		Debug.Log("allowing fling");
		canFling = true;
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
		if (!isEnabled) {
			_input = Vector2.zero;
			pullVector = Vector3.zero;
		}

	}

	public void ApplyLeftStickInput(Vector2 input) 
	{
		_input = input;
	}

	public void ApplyRightStickInput(Vector2 input) {}

	public void DoActionAlpha() {}

	public string Name() 
	{
		return "Rubber Band " + name;
	}
}