﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Orbiter : MonoBehaviour
{

    public float angle;
    public Transform thingToOrbit;
    public float orbitRadius = 1;
    public Vector3 orbitOffset;
    public float lerpSpeed = 8;
    private Vector3 _orbitPos;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // This function is public so unity events can interface with it
    public void SetThingToOrbit(GameObject thing)
    {
        thingToOrbit = thing.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!thingToOrbit) return;
        Vector3 circle = new Vector3(orbitRadius * Mathf.Cos(angle), 0, orbitRadius * Mathf.Sin(angle));
        _orbitPos = thingToOrbit.position + orbitOffset + circle;
        transform.position = Vector3.Lerp(transform.position, _orbitPos, Time.deltaTime * lerpSpeed);
    }
}
