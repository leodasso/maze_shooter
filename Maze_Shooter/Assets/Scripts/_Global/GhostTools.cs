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


	/// <summary>
	/// For things that instantiate in onDisable or onDestroy, this checks if it's safe.
	/// If the editor is destroying things because it's exiting playmode, this will return false.
	/// For builds, always returns true
	/// </summary>
	public static bool SafeToInstantiate(GameObject caller = null)
	{
		#if UNITY_EDITOR
		if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode && UnityEditor.EditorApplication.isPlaying ) {
			Debug.Log("Tried to instantiate but editor was about to exit playmode, so ignoring instantiate.", caller);
			return false;
		}
		if (!UnityEditor.EditorApplication.isPlaying) {
			Debug.Log("Tried to instantiate but editor isn't in playmode, so ignoring instantiate.", caller);
			return false;
		}
		#endif
		return true;
	}
}
