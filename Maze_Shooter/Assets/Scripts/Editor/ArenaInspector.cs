using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector.Editor;

[CustomEditor(typeof(Arena))]
public class ArenaInspector : OdinEditor
{

    protected virtual void OnSceneGUI()
    {	
        Arena arena = (Arena)target;

		EditorGUI.BeginChangeCheck();

		// store all the changed values in here so we can apply them AFTER doing undo record object
		List<Vector3> newPositions = new List<Vector3>();
		List<float> newRadius = new List<float>();

		foreach ( var circle in arena.arenaCircles)
		{
			Handles.matrix = Matrix4x4.TRS(arena.transform.position, arena.transform.rotation, Vector3.one);

			// draw x offset arrow
			newPositions.Add(Handles.PositionHandle(circle.offset, Quaternion.identity));

			// Draw radius
			Handles.color = Color.yellow;
			Vector3 scalePos = Handles.Slider(circle.offset + Vector3.back * circle.radius, Vector3.forward);
			newRadius.Add(Vector3.Distance(scalePos, circle.offset));
		}

		if (EditorGUI.EndChangeCheck()){
			Debug.Log("There were changes homie");
			Undo.RecordObject(arena, "Adjust arena");

			for (int i = 0; i < arena.arenaCircles.Count; i++)
			{
				arena.arenaCircles[i].offsetX = newPositions[i].x;
				arena.arenaCircles[i].offsetZ = newPositions[i].z;

				arena.arenaCircles[i].radius = newRadius[i];
			}
		}
    }
}
