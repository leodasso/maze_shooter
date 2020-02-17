using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class OrbiterGroup : MonoBehaviour
{
    public List<Orbiter> orbiters = new List<Orbiter>();
    public float speed = 45;

    float _offset;
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (var orbiter in orbiters)
        {
            orbiter.thingToOrbit = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        _offset += Time.deltaTime * speed;
        
        for (int i = 0; i < orbiters.Count; i++)
        {
            var orbiter = orbiters[i];
            if (!orbiter) continue;
            float angle = 360 * (i / (float) orbiters.Count);
            orbiter.angle = _offset + angle;
        }
    }

    public void AddOrbiter(Orbiter newOrbiter)
    {
        if (orbiters.Contains(newOrbiter)) return;
        newOrbiter.thingToOrbit = transform;
        orbiters.Add(newOrbiter);
    }
}
