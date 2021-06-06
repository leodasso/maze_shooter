using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

public class GhostMenu 
{
	[MenuItem("Hauntii/Set Scene Cam")]
	public static void SetCameraAngle() 
	{
		Camera cam = Camera.main;
		if (!cam) return;

		Quaternion camRotation = cam.transform.rotation;

		SceneView.lastActiveSceneView.rotation = camRotation;
        SceneView.lastActiveSceneView.Repaint();
	}

	[MenuItem("Hauntii/Game Master")]
	public static void SelectGameMaster()
	{
		Selection.activeObject = AssetDatabase.LoadMainAssetAtPath("Assets/Resources/game master.asset");
	}

	[MenuItem("Hauntii/Show Scene Gates")]
	public static void HilightGates()
	{
		SetSearchFilter("Gate", FILTERMODE_TYPE);
	}


	// System.Reflection black magic for setting search field. Thank you stack overflow
	// https://stackoverflow.com/questions/29575964/setting-a-hierarchy-filter-via-script?newreg=cb894508dece4a8eb3d8eadb4397ed96
	const int FILTERMODE_ALL = 0;
	const int FILTERMODE_NAME = 1;
	const int FILTERMODE_TYPE = 2;

	public static void SetSearchFilter(string filter, int filterMode) 
	{
		SearchableEditorWindow hierarchy = null;
		SearchableEditorWindow[] windows = (SearchableEditorWindow[])Resources.FindObjectsOfTypeAll (typeof(SearchableEditorWindow));

		foreach (SearchableEditorWindow window in windows) {

			if(window.GetType().ToString() == "UnityEditor.SceneHierarchyWindow") {

				hierarchy = window;
				break;
			}
		}

		if (hierarchy == null)
			return;

		MethodInfo setSearchType = typeof(SearchableEditorWindow).GetMethod("SetSearchFilter", BindingFlags.NonPublic | BindingFlags.Instance);         
		object[] parameters = new object[]{filter, filterMode, false, false};

		setSearchType.Invoke(hierarchy, parameters);
	}
}
