using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NoiseGenerator))]
public class NoiseMoverSimple : MonoBehaviour
{
	public NoiseGenerator noiseGenerator;
	public Vector3 movementAmount = Vector3.one;

	Vector3 _scaledVector;
	Vector3 _init;

    // Start is called before the first frame update
    void Start()
    {   
		_init = transform.localPosition;
	}

    // Update is called once per frame
    void Update()
    {
		if (!noiseGenerator) return;
		_scaledVector = Vector3.Scale(noiseGenerator.noise, movementAmount);
		transform.localPosition = _init + _scaledVector;
    }
}
