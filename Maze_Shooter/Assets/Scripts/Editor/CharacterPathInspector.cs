using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;

[CustomEditor(typeof(CharacterPath))]
public class PositionHandleExampleEditor : OdinEditor
{
    protected virtual void OnSceneGUI()
    {
        CharacterPath charPath = (CharacterPath)target;

		Vector3[] pathPositions = new Vector3[charPath.pathPoints.Count];

        EditorGUI.BeginChangeCheck();
		for (int i = 0; i < charPath.pathPoints.Count; i++) {

			Vector3 thisPos = PathPointPos(charPath, i);

			// draw move gizmo for path point
			pathPositions[i] = Handles.PositionHandle(thisPos, Quaternion.identity);

			// draw the label
			Handles.Label(thisPos, i.ToString(), new GUIStyle());

			// draw lines
			if (i > 0) {
				Vector3 prevPos = PathPointPos(charPath, i-1);
				Handles.DrawDottedLine(prevPos, thisPos, 5);
			}
		}

		// connect edges if the path is looped
		if (charPath.looped) 
			Handles.DrawDottedLine(
				PathPointPos(charPath, 0), 
				PathPointPos(charPath, charPath.pathPoints.Count - 1), 5);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(charPath, "Change Character Path Positions");

			for (int i = 0; i < charPath.pathPoints.Count; i++) {
				Vector3 newPos = charPath.transform.InverseTransformPoint(pathPositions[i]);
				charPath.pathPoints[i].pos = newPos;
			}
		}
    }

	Vector3 PathPointPos(CharacterPath path, int index) => path.transform.TransformPoint(path.pathPoints[index].pos);
}