﻿using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;

[TypeInfoBox("Tools for animating scale. The scaling curve should always be small on left / big on right. \n Scale to small moves from the right end of curve to left, and scale to big goes left to right. ")]
public class Scaler : MonoBehaviour
{
	public enum StartScale {
		Default,
		BigScale,
		SmallScale
	}

	public StartScale startScale = StartScale.Default;
	public AnimationCurve scalingCurve = AnimationCurve.Linear(0,0, 1, 1);

	void Start() 
	{
		if (startScale == StartScale.SmallScale) 
			transform.localScale = Vector3.one * scalingCurve.Evaluate(0);

		if (startScale == StartScale.BigScale) 
			transform.localScale = Vector3.one * scalingCurve.Evaluate(scalingCurve.Duration());
	}

	[ButtonGroup]
	public void ScaleToSmall() {
		StartCoroutine(ScaleSequence(scalingCurve.Duration(), 0));
	}

	[ButtonGroup]
	public void ScaleToBig() {
		StartCoroutine(ScaleSequence(0, scalingCurve.Duration()));
	}

	IEnumerator ScaleSequence(float startTime, float endTime) 
	{
		float progress = 0;
		float duration = Mathf.Abs(startTime - endTime);
		float scale = 0;
		while (progress <= 1) {
			progress += Time.unscaledDeltaTime / duration;
			float t = Mathf.Lerp(startTime, endTime, progress);
			scale = scalingCurve.Evaluate(t);
			transform.localScale = Vector3.one * scale; 
			yield return null;
		}

		scale = scalingCurve.Evaluate(endTime);
	}
}
