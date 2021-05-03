using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;

[CustomEditor(typeof(CharacterPath))]
public class CharacterPathInspector : OdinEditor
{
	static float handleSize = .25f;
	static Texture2D guiBg;


	protected override void OnEnable()
	{
		base.OnEnable();
		guiBg = Resources.Load("defaultBg") as Texture2D;
	}

	public static void DrawCharacterPathInspector(CharacterPath charPath, bool allowEdit = true) 
	{
		GUIStyle labelStyle = new GUIStyle();
		labelStyle.normal.textColor = Color.black;
		labelStyle.normal.background = guiBg;
		labelStyle.padding = new RectOffset(4, 2, 2, 2);

		List<Vector3> pathPositions = new List<Vector3>();

		int indexToDelete = -1;

		// Track mouseUp events so that points can be deleted on mouse up
		bool mouseUp = false;
		Event e = Event.current;
		if (e.type == EventType.MouseUp) mouseUp = true;

        EditorGUI.BeginChangeCheck();
		for (int i = 0; i < charPath.pathPoints.Count; i++) {

			Handles.color = Color.yellow;

			Vector3 thisPos = PathPointPos(charPath, i);

			// draw the circle for this pathPos
			Handles.DrawWireArc(thisPos, Vector3.up, Vector3.left, 360, handleSize);

			// draw move gizmo for path point
			if (allowEdit)
				pathPositions.Add(Handles.PositionHandle(thisPos, Quaternion.identity));

			// draw the label
			string labelText = "(" + i.ToString() + ")";
			Handles.Label(thisPos, labelText, labelStyle);


			if (i > 0) {
				Vector3 prevPos = PathPointPos(charPath, i-1);

				// If a point is too close to it's neighbor, we draw a red circle and mark it
				// to be deleted. When user lets mouse up, it will be deleted
				if (Vector3.Distance(prevPos, thisPos) < .5f *  HandleUtility.GetHandleSize(thisPos)) {
					Handles.color = Color.red;
					Handles.DrawWireArc(thisPos, Vector3.up, Vector3.left, 360, handleSize * 2);
					indexToDelete = i;
				}

				// if not within delete range, draw all the normal stuff
				else {
					// draw line between points
					Handles.DrawDottedLine(prevPos, thisPos, 5);

					if (allowEdit) {
						// button for adding a new point in the path
						Handles.color = Color.green;
						Vector3 addBtnPos = (thisPos + prevPos) / 2;

						float screenSize =  HandleUtility.GetHandleSize(addBtnPos);
						if (Handles.Button(addBtnPos, Quaternion.identity, screenSize * .1f, screenSize * .3f, Handles.CircleHandleCap)) {
							charPath.InsertNewPoint(i, addBtnPos);
							pathPositions.Insert(i, addBtnPos);
						}

						// button label
						Handles.Label(addBtnPos, "+", labelStyle);
					}
				}
			}
		}

		Handles.color = Color.yellow;

		// connect edges if the path is looped
		if (charPath.looped) 
			Handles.DrawDottedLine(
				PathPointPos(charPath, 0), 
				PathPointPos(charPath, charPath.pathPoints.Count - 1), 5);


		if (indexToDelete >= 0 && mouseUp && allowEdit) 
			charPath.RemovePoint(indexToDelete);

        if (EditorGUI.EndChangeCheck() && allowEdit)
        {
            Undo.RecordObject(charPath, "Change Character Path Positions");

			for (int i = 0; i < charPath.pathPoints.Count; i++) {
				Vector3 newPos = charPath.transform.InverseTransformPoint(pathPositions[i]);
				charPath.pathPoints[i].pos = newPos;
			}
		}
	}

    protected virtual void OnSceneGUI()
    {	
        CharacterPath charPath = (CharacterPath)target;
		DrawCharacterPathInspector(charPath);
    }

	static Vector3 PathPointPos(CharacterPath path, int index) => path.transform.TransformPoint(path.pathPoints[index].pos);
}