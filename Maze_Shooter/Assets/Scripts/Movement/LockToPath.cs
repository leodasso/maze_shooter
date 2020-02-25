using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[ExecuteAlways]
public class LockToPath : MonoBehaviour
{
    public CinemachinePathBase path;
    
    [Tooltip("The normalized position along the given path"), Range(0, 1)]
    public float position = .5f;
    
    // Update is called once per frame
    void Update()
    {
        if (!path) return;
        transform.position = path.EvaluatePositionAtUnit(position, CinemachinePathBase.PositionUnits.Normalized);
    }
}