using UnityEngine;
using Cinemachine;

[ExecuteAlways]
public class PathHelper : MonoBehaviour
{
	public CinemachinePath path;
	public Transform start;
	public Transform end;

    // Update is called once per frame
    void Update()
    {
        if (!path) return;
		if (start) {
			path.m_Waypoints[0].position = transform.InverseTransformPoint(start.position);
		}

		if (end) {
			int lastPoint = path.m_Waypoints.Length - 1;
			path.m_Waypoints[lastPoint].position = transform.InverseTransformPoint(end.position);
		}
    }
}