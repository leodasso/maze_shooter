using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShootyGhost;
using Sirenix.OdinInspector;

[AddComponentMenu("Character Controllers/Generic Dash")]
public class GenericDash : MovementMod
{
	public AnimationCurve dashSpeedMultiplier = AnimationCurve.Constant(0, 1, 1);

	[Tooltip("Sends events 'dash', 'dashFinish'")]
	public PlayMakerFSM playMaker;

	float _dashMultiplier = 1;
	Vector3 _dashDirection;
	bool _dashing;

	public void Dash() 
	{
		if (_dashing) return;
		_dashing = true;
		_dashDirection = direction.magnitude < .1f ? lastDirection : direction;
		_dashDirection.Normalize();
		StartCoroutine(DashRoutine());
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
		_dashMultiplier = 1;
	}

	public override Vector3 ModifyVelocity(Vector3 input)
	{
		return input * _dashMultiplier;
	}

	public override void DoActionAlpha()
	{
		playMaker.SendEvent("dash");
	}
}