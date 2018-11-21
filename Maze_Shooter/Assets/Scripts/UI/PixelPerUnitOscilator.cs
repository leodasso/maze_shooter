using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class PixelPerUnitOscilator : MonoBehaviour
{
	public float startingPpu = 5;
	public float oscillationAmount = 5;
	public float oscillationFrequency = 1;
	Canvas _canvas;
	float _t;

	// Use this for initialization
	void Start ()
	{
		_canvas = GetComponent<Canvas>();
		_t = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		_t += Time.unscaledDeltaTime * oscillationFrequency;
		float output = Mathf.Sin(_t);

		float ppu = startingPpu + output * oscillationAmount;
		_canvas.referencePixelsPerUnit = ppu;
	}
}
