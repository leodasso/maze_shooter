﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SpaceMovement : MonoBehaviour
{
	[Range(0, 1)]
	public float progress = 0;

	[Tooltip("Cosmetic - for finessing how the star looks throughout it's motion")]
	public AnimationCurve progressCurve = AnimationCurve.Linear(0, 0, 1, 1);

	[Tooltip("Color of the star as it moves through its gradient")]
	public Gradient progressGradient;

	public List<SpriteRenderer> renderersToColor = new List<SpriteRenderer>();


	[Tooltip("Optional perlin noise to make movement more random")]
	public NoiseGenerator noise;

	[ShowIf("UseNoise")]
	public AnimationCurve noiseIntensity = AnimationCurve.Linear(0, 0, 1, 1);

	Vector3 _finalPos;
	Transform _finalPosTransform;
	// randomly assigned at start - determines the direction of movement of the sample point along the perlin plane
    Vector2 _noiseSampleDirection;

	Vector3 ActualFinalPos => _finalPosTransform ? _finalPosTransform.localPosition : _finalPos; 

	bool UseNoise => noise != null;

    // Start is called before the first frame update
    void Start()
    {
        _finalPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
		foreach( SpriteRenderer r in renderersToColor) 
			r.color = progressGradient.Evaluate(progress);

		float curvedProgress = progressCurve.Evaluate(progress);
        transform.localPosition = Vector3.LerpUnclamped(Vector3.zero, ActualFinalPos, curvedProgress);

		if (UseNoise) 
			transform.Translate(noise.noise * noiseIntensity.Evaluate(progress));
    }

	public void SetDestinationObject(Transform t) {
		_finalPosTransform = t;
	}

	public void PlayAnimation(float beginningValue, float endValue, float duration, Action onAnimComplete = null) 
	{
		StartCoroutine(PlayAnimSequence(beginningValue, endValue, duration, onAnimComplete));
	}

	IEnumerator PlayAnimSequence(float beginningValue, float endValue, float duration, Action onAnimComplete) 
	{
		float lerp = 0;
		while (lerp < 1) {
			
			progress = Mathf.Lerp(beginningValue, endValue, lerp);
			lerp += Time.unscaledDeltaTime / duration;
			yield return null;
		}
		progress = endValue;

		if (onAnimComplete != null) onAnimComplete.Invoke();
	}

	void RandomizeNoise()
    {
        _noiseSampleDirection = UnityEngine.Random.onUnitSphere;
    }
}
