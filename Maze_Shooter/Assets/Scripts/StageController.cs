using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using Sirenix.OdinInspector;

public class StageController : MonoBehaviour
{
	[AssetsOnly]
	public Stage stage;

	// Use this for initialization
	IEnumerator Start ()
	{
		// Make sure we have a stage, and throw an error if we dont
		if (stage == null)
		{
			Debug.LogError("No stage is referenced in stage controller!");
			enabled = false;
			yield break;
		}

		GameMaster.Get().currentStage = stage;
		
		// Raise the immediate events
		foreach (var e in stage.immediateEvents)
			e.Raise();
		
		Time.timeScale = 0;
		yield return new WaitForSecondsRealtime(stage.startingDelay.Value);
		
		// Raise the delayed events
		foreach (var e in stage.postStartingDelayEvents)
			e.Raise();
		
		Time.timeScale = 1;
	}
}