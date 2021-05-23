using UnityEngine;
using Arachnid;

public class BezierTest : MonoBehaviour
{
	public Transform point1;
	public Transform anchor1;

	public Transform point2;
	public Transform anchor2;

	void OnDrawGizmos()
	{
		if (!point1 || !anchor1 || !point2 || !anchor2) return;

		Vector3 prevPoint = Math.GetBezier(0, point1.position, anchor1.position, anchor2.position, point2.position);
		for (float i = .02f; i < 1; i += .02f)
		{
			Vector3 thisPoint = Math.GetBezier(i, point1.position, anchor1.position, anchor2.position, point2.position);
			Gizmos.DrawLine(prevPoint, thisPoint);
			prevPoint = thisPoint;
		}
	}
}