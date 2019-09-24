using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

[TypeInfoBox("Links nodes in the world map to actual stages.")]
public class StageCrystal : MonoBehaviour
{
	[TabGroup("main")]
	public Stage stage;
	[TabGroup("main"), Tooltip("Optional stage description UI.")]
	public StageDescriptor stageDescriptor;
	
	[TabGroup("main"), Tooltip("Optional masker - will reveal interior when the stage is selected")]
	public WorldMapMask linkedMask;

	[ShowInInspector, SerializeField, TabGroup("main")]
	float _stageLoadDelay = 1;


	
	[TabGroup("events")]
	[ShowInInspector, SerializeField]
	List<GameEvent> _onStageEnter = new List<GameEvent>();
	
	[TabGroup("events"), Tooltip("Selected meaning when the player lands on this particular node in the world map.")]
	[ShowInInspector, SerializeField]
	List<GameEvent> _onSelected = new List<GameEvent>();
	
	[TabGroup("events")]
	[ShowInInspector, SerializeField]
	List<GameEvent> _onDeselected = new List<GameEvent>();
	
	[TabGroup("events"), Tooltip("Selected meaning when the player lands on this particular node in the world map.")]
	[ShowInInspector, SerializeField]
	UnityEvent _onSelectedEvent;
	
	[ShowInInspector, SerializeField]
	[TabGroup("events")]
	UnityEvent _onDeselectedEvent;
	

	public void EnterStage()
	{
		foreach (var e in _onStageEnter) e.Raise();
		stage?.Load(_stageLoadDelay);
	}

	public void SetSelected(bool selected)
	{
		if (selected)
		{
			linkedMask?.Appear();
			
			if (stageDescriptor)
			{
				stageDescriptor.stage = stage;
				stageDescriptor.Refresh();
				stageDescriptor.Show();
			}

			_onSelectedEvent.Invoke();
			foreach (var e in _onSelected) e.Raise();
		}
		else
		{
			linkedMask?.Disappear();
			
			stageDescriptor?.Hide();
			_onDeselectedEvent.Invoke();
			foreach (var e in _onDeselected) e.Raise();
		}
	}
}
