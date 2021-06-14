using Arachnid;
using UnityEngine;
using Sirenix.OdinInspector;

public class KinematicProjectile : Projectile
{
	[Tooltip("Speed multiplier over the lifetime of this projectile")]
	public AnimationCurve speedMultiplier = AnimationCurve.Linear(0, 1, 1, 1);

	[Tooltip("After force and everything is added on, speed can't exceed this value")]
	public float maxSpeed = 150;

	[ToggleLeft, Space, Tooltip("Add a world space force over the course of the lifetime of the projectile")]
	public bool addGlobalForce;
	[ShowIf("addGlobalForce"), Tooltip("This is a world-space force")]
	public Vector3 globalForce;

	[ToggleLeft, Tooltip("Add a force in the direction of the projectile movement over the course of the lifetime of the projectile.")] 
	public bool addForwardForce;
	[ShowIf("addForwardForce"), Tooltip("Force added in the local direction of the projectile. A negative value here will slow the bullet down")]
	public FloatReference forwardForce;
	
	float _localSpeed;
	// The velocity added from global force over lifetime.
	Vector3 globalVelocityAdd;
	Vector3 _totalVelocity;

	protected override void OnEnable()
	{
		base.OnEnable();
		_localSpeed = speed.Value;
	}

	// Update is called once per frame
	protected override void Update ()
	{
		base.Update();

		// Speed and force calculations
		if (addForwardForce) 
			_localSpeed += forwardForce.Value * Time.deltaTime;

		// Get a world space vector from the local speed
		_totalVelocity = fireDirection * _localSpeed * speedMultiplier.Evaluate(_lifetimeTimer);

		Debug.DrawRay(transform.position, _totalVelocity, Color.yellow, 10);

		
		if (addGlobalForce)
			globalVelocityAdd += globalForce * Time.deltaTime;

		// Clamp the total velocity to max speed
		_totalVelocity += globalVelocityAdd;
		_totalVelocity = Vector3.ClampMagnitude(_totalVelocity, maxSpeed);
		

		transform.Translate(_totalVelocity * Time.deltaTime, Space.World);
	}
	
	
	// ---- These functions are here so that UnityEvents can interface with the values ---
	public void EnableForwardForce(bool isEnabled)
	{
		addForwardForce = isEnabled;
	}
}
