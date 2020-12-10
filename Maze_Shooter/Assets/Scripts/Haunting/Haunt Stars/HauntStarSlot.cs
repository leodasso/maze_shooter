﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HauntStarSlot : MonoBehaviour
{
	[Range(0, 1)]
	public float progress = 0;

	[Tooltip("Cosmetic - for finessing how the star looks throughout it's motion")]
	public AnimationCurve progressCurve = AnimationCurve.Linear(0, 0, 1, 1);

	[Tooltip("Color of the star as it moves through its gradient")]
	public Gradient progressGradient;

	public List<SpriteRenderer> renderersToColor = new List<SpriteRenderer>();

	Vector3 _finalPos;

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
        transform.localPosition = Vector3.LerpUnclamped(Vector3.zero, _finalPos, curvedProgress);
    }
}
