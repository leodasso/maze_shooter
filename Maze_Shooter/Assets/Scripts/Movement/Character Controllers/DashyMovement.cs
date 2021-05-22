using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Character Controllers/Dashy")]
public class DashyMovement : MovementBase
{
	[Tooltip("How quickly I stop when there's no")]
	public float drag = 10;

	[Tooltip("Vertical velocity applied when bouncing")]
	public float bounceHeightSpeed = 15;
	public float bounceHorizontalSpeed = 10;

	public AnimationCurve dashSpeedMultiplier = AnimationCurve.Constant(0, 1, 1);
	[Range(0, 1), Tooltip("Percentage of the dash in which spikes are spawned. ")]
	public float spawnSpikesPercentage = .85f;
	public float dashCooldown = 2;

	[Tooltip("If collides with objects with these tags while dashing, won't bounce.")]
	public List<string> omitBounceTags = new List<string>();

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

	protected override void CalculateTotalVelocity()
	{
		if (_dashing) {
			direction = _dashDirection;
			totalVelocity = _dashDirection * TotalMaxSpeed * _dashMultiplier;
		}
		else
			base.CalculateTotalVelocity();
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
		while (t < dashSpeedMultiplier.Duration() * spawnSpikesPercentage) {
			_dashMultiplier = dashSpeedMultiplier.Evaluate(t);
			t += Time.deltaTime;
			yield return null;
		}

		playMaker.SendEvent("dashFinish");

		float remainingDashTime = dashSpeedMultiplier.Duration() * (1 - spawnSpikesPercentage);
		yield return new WaitForSeconds(remainingDashTime);

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
		if (_dashing) {
			if (omitBounceTags.Contains(other.gameObject.tag)) return;
			playMaker.SendEvent("dashBump");
		}
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