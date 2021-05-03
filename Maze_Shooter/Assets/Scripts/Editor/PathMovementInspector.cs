using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;

[CustomEditor(typeof(PathMovement))]
public class PathMovementInspector : OdinEditor
{
	static float handleSize = .25f;
	static Texture2D guiBg;


	protected override void OnEnable()
	{
		base.OnEnable();
		guiBg = Resources.Load("defaultBg") as Texture2D;
	}

    protected virtual void OnSceneGUI()
    {	
		PathMovement pathMovement = (PathMovement)target;
		if (pathMovement.path)
			CharacterPathInspector.DrawCharacterPathInspector(pathMovement.path, false);
    }

}