using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;

[CustomEditor(typeof(PathMovement))]
public class PathMovementInspector : OdinEditor
{
	static float handleSize = .25f;
	static float arrowSize = 3;


	protected override void OnEnable()
	{
		base.OnEnable();
		CharacterPathInspector.guiBg = Resources.Load("defaultBg") as Texture2D;
	}

    protected virtual void OnSceneGUI()
    {	
		PathMovement pathMovement = (PathMovement)target;
		if (pathMovement.path)
			CharacterPathInspector.DrawCharacterPathInspector(pathMovement.path, false, pathMovement.destinationRadius, pathMovement.pathEvents);

		Vector3 lookDir = pathMovement.VectorToNext();
		Vector3 arrowPos = pathMovement.transform.position + lookDir - lookDir.normalized * arrowSize;

		// Draw arrow pointing in the direction of movement
		if (lookDir.magnitude > Mathf.Epsilon) {
			Handles.color = Color.blue;
			Handles.ArrowHandleCap(
				0,
				arrowPos,
				Quaternion.LookRotation(lookDir),
				arrowSize,
				EventType.Repaint
			);
		}
    }

}