using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NoiseGenerator))]
public class RandomMovement : MovementBase
{
	[SerializeField]
	AnimationCurve noiseMapping = AnimationCurve.Linear(0, 0, 1, 1);
	NoiseGenerator noiseGenerator;
	float angle;

	void OnDrawGizmosSelected()
	{
		Gizmos.DrawLine(transform.position, transform.position + direction * 10);
	}

	protected override void Start()
	{
		noiseGenerator = GetComponent<NoiseGenerator>();
		base.Start();
	}

	protected override void FixedUpdate()
	{
		float noiseOutput = noiseGenerator.normalizedOutput;
		angle = Mathf.LerpUnclamped(-180, 180, noiseMapping.Evaluate(noiseOutput));

		Vector2 flatDir = Arachnid.Math.DegreeToVector2(angle);

		direction = Arachnid.Math.Project2Dto3D(flatDir);

		base.FixedUpdate();
	}
}