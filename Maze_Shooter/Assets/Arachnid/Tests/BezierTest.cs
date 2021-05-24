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

		GizmoExtensions.DrawBezier(point1.position, anchor1.position, anchor2.position, point2.position);
	}
}