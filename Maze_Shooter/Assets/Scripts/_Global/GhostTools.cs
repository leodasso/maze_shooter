using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GhostTools 
{
	
	/// <summary>
	/// Casts down and returns the point below the given point that touches the ground.
	/// </summary>
	/// <returns>The point on the ground directly below this</returns>
	public static Vector3 GroundPoint(Vector3 point) 
	{
		RaycastHit hit;
		if (Physics.Raycast(point, Vector3.down, out hit, 50, LayerMask.GetMask("Ground"))) {
			return hit.point;
		}
		return point;
	}
}
