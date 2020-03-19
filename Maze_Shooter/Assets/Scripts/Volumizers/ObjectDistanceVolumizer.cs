using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Arachnid;
using UnityEngine;

public class ObjectDistanceVolumizer : Volumizer
{
    public Collection objectsToCheck;
    public float minDistance = 0;
    public float maxDistance = 10;
    
    float _distance;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Find the nearest element's distance
        float nearestDist = 99999;
        foreach (var element in objectsToCheck.elements)
        {
            float thisDistance = Vector3.Distance(transform.position, element.transform.position);
            if (thisDistance < nearestDist)
            {
                nearestDist = thisDistance;
            }
        }

        float range = Mathf.Abs(maxDistance - minDistance);
        UpdateNormalizedValue((nearestDist - minDistance) / range);
        UpdateVolumes();
    }
}
