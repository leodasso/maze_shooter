using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GhostMenu 
{
	[MenuItem("Ghost/Set Scene Cam")]
	public static void SetCameraAngle() 
	{
		Camera cam = Camera.main;
		if (!cam) return;

		Quaternion camRotation = cam.transform.rotation;

		SceneView.lastActiveSceneView.rotation = camRotation;
        SceneView.lastActiveSceneView.Repaint();
	}
}
