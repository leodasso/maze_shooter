using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpToTarget : MonoBehaviour
{
	[SerializeField]
	TargetFinder targetFinder;

	[Tooltip("Time in seconds to complete the lerp")]
	public float lerpTime = 2;

	[Tooltip("The max distance from start to finish. If a longer lerp is requested, its magnitude will be clamped")]
	public float maxDistance = 250;

	public void DoLerp()
	{
		Vector3 endPos = targetFinder.currentTarget.transform.position;
		Vector3 lerpVector = Vector3.ClampMagnitude(endPos - transform.position, maxDistance);

		StartCoroutine(LerpRoutine(lerpVector));
	}

	IEnumerator LerpRoutine(Vector3 lerpVector)
	{
		Vector3 startPos = transform.position;
		float progress = 0;
		while (progress < 1)
		{
			transform.position = Vector3.Lerp(startPos, startPos + lerpVector, progress);
			progress += Time.deltaTime / lerpTime;
			yield return null;
		}

		transform.position = startPos + lerpVector;
	}
}