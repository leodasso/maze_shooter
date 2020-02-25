using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;

[ExecuteAlways]
public class PathLineRenderer : MonoBehaviour
{
    public CinemachinePathBase path;
    public LineRenderer lineRenderer;
    [MinValue(.05f)]
    public float minPointDistance = .5f;

    float _progress;
    int _positionIndex;
    int _positionCount;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!path || !lineRenderer) return;

        _progress = 0;
        _positionIndex = 0;
        _positionCount = Mathf.CeilToInt(path.PathLength / minPointDistance);
        lineRenderer.positionCount = _positionCount + 1;
        
        while (_positionIndex < _positionCount)
        {
            var pos = path.EvaluatePositionAtUnit(_progress, CinemachinePathBase.PositionUnits.Distance);
            lineRenderer.SetPosition(_positionIndex, pos);
            _progress += minPointDistance;
            _positionIndex++;
        }
        
        // Manually place the last position so it always ends on the same point
        var finalPos = path.EvaluatePositionAtUnit(1, CinemachinePathBase.PositionUnits.Normalized);
        lineRenderer.SetPosition(_positionIndex, finalPos);
    }
}
