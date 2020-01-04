using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using UnityEngine.Events;

public class EventOnAwake : MonoBehaviour
{
	//[Tooltip("All the stages in this list will need to be complete")]
	//public List<Stage> prerequisiteStages = new List<Stage>();
	
	//[Tooltip("If any of the stages in this list are complete, this wont trigger")]
	//public List<Stage> notCompleteYet = new List<Stage>();
	public UnityEvent eventToInvoke;
	public List<GameEvent> gameEvents = new List<GameEvent>();
	
	void Awake()
	{
		// TODO convert prereq list to list of saveable toggles
		/*
		foreach (var stage in prerequisiteStages)
			if (!stage.IsComplete()) return;

		foreach (var stage in notCompleteYet)
			if (stage.IsComplete()) return;
		*/
		
		eventToInvoke.Invoke();
		foreach (var e in gameEvents) e.Raise();
	}
}