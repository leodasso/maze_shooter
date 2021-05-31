using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("Character Controllers/Dashy")]
public class DashyMovement : MovementBase
{
	public AnimationCurve dashSpeedMultiplier = AnimationCurve.Constant(0, 1, 1);
	[Range(0, 1), Tooltip("Percentage of the dash in which spikes are spawned. ")]
	public float spawnSpikesPercentage = .85f;
	public float dashCooldown = 2;

	[Tooltip("Sends events 'dash', 'dashFinish', 'cooldownFinish', 'dashBump'")]
	public PlayMakerFSM playMaker;

	bool _dashing = false;
	bool _cooldown = false;
	float _dashMultiplier = 1;
	Vector3 _dashDirection;

	protected override void Start()
	{
		base.Start();
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
}