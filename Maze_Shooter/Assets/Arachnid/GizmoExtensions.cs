using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
