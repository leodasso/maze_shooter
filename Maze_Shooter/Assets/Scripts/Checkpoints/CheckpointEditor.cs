using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
 // https://forum.unity.com/threads/how-to-make-buttons-in-scene-view.1009861/

public class CheckpointTool : EditorWindow
{

	static List<Checkpoint> allCheckPoints = new List<Checkpoint>();
	const int buttonHeight = 24;
	static bool enabled;

    [MenuItem("Hauntii/Show Checkpoints &1")]
    public static void Toggle()
    {
		if (!enabled) {
			SceneView.duringSceneGui += OnSceneGUI;
			GetCheckpoints();
			enabled = true;
		}else {
			enabled = false;
			SceneView.duringSceneGui -= OnSceneGUI;
		}
    }
 

	static void GetCheckpoints()
	{
		allCheckPoints.Clear();
		allCheckPoints.AddRange(FindObjectsOfType<Checkpoint>());
	}
 
    static void OnSceneGUI(SceneView sceneview)
    {
		GUIStyle boxStyle = new GUIStyle("box");
        Handles.BeginGUI();
		float boxHeight =  20 + buttonHeight * allCheckPoints.Count;
		GUILayout.BeginArea(new Rect(10, 10, 250, boxHeight), boxStyle);

		GUILayout.Label("Spawn Checkpoints");
 
		foreach ( var checkpoint in allCheckPoints ) {
			if (GUILayout.Button("Spawn from " + checkpoint.name)) {
				checkpoint.SetAsActiveCheckpoint();
				EditorGUIUtility.PingObject(checkpoint);

 				Selection.activeGameObject = checkpoint.gameObject;
 				SceneView.FrameLastActiveSceneView();
				 SceneView.lastActiveSceneView.size = 30;

			}
		}


		GUILayout.EndArea();
 
        Handles.EndGUI();
    }
}