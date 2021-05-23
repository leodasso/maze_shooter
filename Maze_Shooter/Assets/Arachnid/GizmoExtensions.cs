using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arachnid;

public static class GizmoExtensions
{
    public static void DrawCircle(Vector3 pos, float radius)
    {
		for (int i = 0; i < 360; i+= 10) {
			Gizmos.DrawLine(pos + PointAtAngle(i, radius), pos + PointAtAngle(i + 10, radius));
		}
    }

	static Vector3 PointAtAngle(float angle, float radius) {
		float radians = Mathf.Deg2Rad * angle;
		float x = radius * Mathf.Cos(radians);
		float z = radius * Mathf.Sin(radians);
		return new Vector3(x, 0, z);
	}

	public static void DrawBezier(Vector3 point1, Vector3 anchor1, Vector3 anchor2, Vector3 point2) 
	{
		Vector3 prevPoint = Math.GetBezier(0, point1, anchor1, anchor2, point2);
		for (float i = .02f; i < 1; i += .02f)
		{
			Vector3 thisPoint = Math.GetBezier(i, point1, anchor1, anchor2, point2);
			Gizmos.DrawLine(prevPoint, thisPoint);
			prevPoint = thisPoint;
		}
	}
}
