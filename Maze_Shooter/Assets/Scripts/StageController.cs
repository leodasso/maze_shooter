using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using Sirenix.OdinInspector;

public class StageController : MonoBehaviour
{
	[AssetsOnly, InlineEditor()]
	public Stage stage;

	void Awake()
	{
		// Make sure we have a stage, and throw an error if we dont
		if (stage == null)
		{
			Debug.LogError("No stage is referenced in stage controller!");
			enabled = false;
			return;
		}

		GameMaster.Get().currentStage = stage;
	}

	// Use this for initialization
	void Start ()
	{
		// Raise the immediate events
		foreach (var e in stage.immediateEvents)
			e.Raise();
	}
}