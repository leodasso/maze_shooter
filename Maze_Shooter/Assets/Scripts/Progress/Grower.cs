using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grower : MonoBehaviour
{
	public AnimationCurve growthSpeedCurve;
	public float growthSpeedMultiplier = 1;
	public float maxScale = 200;

	float _scale = 0;
	float _time = 0;
	
	void Awake()
    {
        transform.localScale = Vector3.one * _scale;
    }

	public void GrowInstantly() {
		_scale = maxScale;
		transform.localScale = Vector3.one * _scale;
	}


	void Update() {

		if (_scale < maxScale) {
			float speed = growthSpeedCurve.Evaluate(_time) * growthSpeedMultiplier;
			_scale += Time.deltaTime * speed;
			transform.localScale = Vector3.one * _scale;

			_time += Time.deltaTime;
		}
	}
}