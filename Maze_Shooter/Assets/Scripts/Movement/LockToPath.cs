using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Sirenix.OdinInspector;

[ExecuteAlways]
public class LockToPath : MonoBehaviour
{
    public CinemachinePathBase path;
    
    [Tooltip("The normalized position along the given path"), Range(0, 1)]
    public float position = .5f;

	[Space, ToggleLeft]
	public bool orientToPath;

	[ShowIf(nameof(orientToPath))]
	public Vector3 offsetRotation;
    
    // Update is called once per frame
    void Update()
    {
        if (!path) return;
        transform.position = path.EvaluatePositionAtUnit(position, CinemachinePathBase.PositionUnits.Normalized);

		if (orientToPath)
		{
			Quaternion offsetQuaternion = Quaternion.Euler(offsetRotation);
			transform.rotation = offsetQuaternion * path.EvaluateOrientationAtUnit(position, CinemachinePathBase.PositionUnits.Normalized);
		}
    }
}