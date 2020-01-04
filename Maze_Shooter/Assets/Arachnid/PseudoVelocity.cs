using System;
using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public class PseudoVelocity : MonoBehaviour {

    [Range(1, 10)]
    [Tooltip("Number of consecutive frames to sample positions from when calculating velocity. Increasing this can smooth out" +
             " velocity readings.")]
    public int velocitySamples = 5;
    
    [ReadOnly]
    public Vector3 velocity;

    readonly List<Vector3> _velocities = new List<Vector3>();
    Vector3 _compoundVelocity = Vector3.zero;
    Vector3 _previousPos = Vector3.zero;

    //Intitializes the sample array
    public void Start()
    {
        OnEnabled();
    }

    public void OnEnabled()
    {
        velocity = Vector3.zero;
        _compoundVelocity = Vector3.zero;
        _previousPos = transform.position;
    }

    public void Update()
    {
        _velocities.Add(PVelocity());
        if (_velocities.Count > velocitySamples) _velocities.RemoveAt(0);

        _compoundVelocity = Vector3.zero;
        for (int i = 0; i < _velocities.Count; i++)
            _compoundVelocity += _velocities[i];

        velocity = _compoundVelocity / _velocities.Count;
    }

    Vector3 PVelocity()
    {
        if (Math.Abs(Time.deltaTime) < Mathf.Epsilon) return Vector3.zero;
        Vector3 returnVel = (transform.position - _previousPos) / Time.deltaTime;
        returnVel = Vector3.ClampMagnitude(returnVel, 9999);
        _previousPos = transform.position;
        return returnVel;
    }
}