using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PrettyLerper : MonoBehaviour
{
	public Transform target;

	[Space]
	public float animationDuration = 2;
	[Tooltip("Controls the height over the progress of the lerp")]
	public AnimationCurve height = AnimationCurve.Linear(0, 0, 1, 1);
	[Tooltip("Be sure this ends at 1,1!")]
	public AnimationCurve horizontalMovement = AnimationCurve.Linear(0, 0, 1, 1);

	[Space, ToggleLeft]
	public bool controlNoiseMovement;

	[ShowIf("controlNoiseMovement")]
	public NoiseGenerator noiseGenerator;

	[ShowIf("controlNoiseMovement")]
	public AnimationCurve noiseSpeed = AnimationCurve.Linear(0, 0, 1, 1);

	[ShowIf("controlNoiseMovement")]
	public AnimationCurve noiseIntensity = AnimationCurve.Linear(0, 0, 1, 1);

	[Space, ToggleLeft]
	public bool circularMovement;

	[ShowIf("circularMovement")]
	public AnimationCurve circleRadius = AnimationCurve.Linear(0, 0, 1, 1);

	[ShowIf("circularMovement"), Tooltip("position in radians")]
	public AnimationCurve circlePos = AnimationCurve.Linear(0, 0, 1, 1);

	public Action onLerpComplete;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

	[Button]
	public void DoLerp() 
	{
		if (!target) return;
		StartCoroutine(DoLerpSequence());
	}
	
	IEnumerator DoLerpSequence() 
	{
		Vector3 startPos = transform.position;
		float progress = 0;
		
		while (progress < 1) {
			progress += Time.unscaledDeltaTime / animationDuration;
			float horizontal = horizontalMovement.Evaluate(progress);
			transform.position = Vector3.Lerp(startPos, target.position, horizontal);
			transform.position += Vector3.up * height.Evaluate(progress);

			if (controlNoiseMovement) {
				noiseGenerator.noiseSpeed = noiseSpeed.Evaluate(progress);
				transform.position += noiseGenerator.noise * noiseIntensity.Evaluate(progress);
			}

			if (circularMovement) {
				float radius = circleRadius.Evaluate(progress);
				float radians = circlePos.Evaluate(progress);

				Vector3 circle = new Vector3(Mathf.Sin(radians), Mathf.Cos(radians), 0) * radius;
				transform.position += circle;
			}

			yield return null;
		}

		if (onLerpComplete != null)
			onLerpComplete.Invoke();
	}
}
