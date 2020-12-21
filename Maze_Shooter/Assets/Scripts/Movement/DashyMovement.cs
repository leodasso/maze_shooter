using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DashyMovement : MovementBase
{
	[Tooltip("How quickly I stop when there's no")]
	public float drag = 10;

	[Tooltip("Vertical velocity applied when bouncing")]
	public float bounceHeightSpeed = 15;
	public float bounceHorizontalSpeed = 10;

	public AnimationCurve dashSpeedMultiplier = AnimationCurve.Constant(0, 1, 1);
	public float dashCooldown = 2;

	[Tooltip("Sends events 'dash', 'dashFinish', 'cooldownFinish', 'dashBump'")]
	public PlayMakerFSM playMaker;

	bool _dashing = false;
	bool _cooldown = false;
	float _dashMultiplier = 1;
	Vector3 _dashDirection;

	RigidbodyConstraints initConstraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
	RigidbodyConstraints bounceConstraints = RigidbodyConstraints.FreezeRotation;

	protected override void Start()
	{
		base.Start();
		_rigidbody.constraints = initConstraints;
	}


    void FixedUpdate()
    {
		Vector3 newVelocity = direction * TotalSpeedMultiplier() * _dashMultiplier;
		if (_dashing) newVelocity = _dashDirection * TotalSpeedMultiplier() * _dashMultiplier;

		if (newVelocity.magnitude > .25f)
			_rigidbody.velocity = new Vector3(newVelocity.x, _rigidbody.velocity.y, newVelocity.z);

		else {
			Vector3 zeroSpeed = new Vector3(0, _rigidbody.velocity.y, 0);
			_rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity, zeroSpeed, Time.fixedDeltaTime * drag);
		}
    }

	public void Dash() 
	{
		if (_dashing || _cooldown) return;
		_dashing = true;
		_dashDirection = direction.magnitude < .1f ? lastDirection : direction;
		_dashDirection.Normalize();
		StartCoroutine(DashRoutine());
		playMaker.SendEvent("dash");
	}

	IEnumerator DashRoutine() 
	{
		float t = 0;
		while (t < dashSpeedMultiplier.Duration()) {
			_dashMultiplier = dashSpeedMultiplier.Evaluate(t);
			t += Time.deltaTime;
			yield return null;
		}

		playMaker.SendEvent("dashFinish");
		_dashing = false;
		_cooldown = true;
		_dashMultiplier = 1;

		// cooldown
		yield return new WaitForSeconds(dashCooldown);
		playMaker.SendEvent("cooldownFinish");
		_cooldown = false;
	}

	public override void DoActionAlpha()
	{
		base.DoActionAlpha();
		Dash();
	}

	protected override void OnCollisionEnter(Collision other)
	{
		base.OnCollisionEnter(other);
		if (_dashing) playMaker.SendEvent("dashBump");
	}

	public void BounceBack() 
	{
		_rigidbody.constraints = bounceConstraints;
		Vector3 bounceVel = -lastDirection * bounceHorizontalSpeed;
		_rigidbody.velocity = new Vector3(bounceVel.x, bounceHeightSpeed, bounceVel.z);
	}

	public void ResetConstraints() {
		_rigidbody.constraints = initConstraints;
	}
}