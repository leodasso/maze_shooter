using Arachnid;
using UnityEngine;
using Sirenix.OdinInspector;

public class PhysicsProjectile : Projectile
{
	public AnimationCurve speedMultiplier = AnimationCurve.Linear(0, 1, 1, 1);
	public FloatReference speed;

	[Tooltip("After force and everything is added on, speed can't exceed this value")]
	public float maxSpeed = 150;

	[ToggleLeft]
	public bool addForceOverLifetime;
	[ShowIf("addForceOverLifetime"), Tooltip("This is a world-space force")]
	public Vector3 forceOverLifetime;

	[ToggleLeft] 
	public bool applyForwardForce;
	[ShowIf("applyForwardForce"), Tooltip("Force added in the local direction of the projectile. A negative value here will slow the bullet down")]
	public FloatReference forwardForceOverLifetime;


	float _localSpeed;
	// The velocity added from global force over lifetime.
	Vector3 _velocity;
	Vector3 _totalVelocity;


	// Update is called once per frame
	protected override void Update ()
	{
		base.Update();

		// Speed and force calculations
		if (applyForwardForce) 
			_localSpeed += forwardForceOverLifetime.Value * Time.deltaTime;

		// Get a world space vector from the local speed
		_totalVelocity = fireDirection * _localSpeed * speedMultiplier.Evaluate(_lifetimeTimer);
		
		transform.Translate(_totalVelocity * Time.deltaTime, Space.World);


		if (addForceOverLifetime)
			_velocity += forceOverLifetime * Time.deltaTime;

		// Clamp the total velocity to max speed
		_totalVelocity += _velocity;
		_totalVelocity = Vector2.ClampMagnitude(_totalVelocity, maxSpeed);
	}
	
	
	// ---- These functions are here so that UnityEvents can interface with the values ---
	public void EnableForwardForce(bool isEnabled)
	{
		applyForwardForce = isEnabled;
	}
}
