using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class StageCrystal : MonoBehaviour
{
	[TabGroup("main")]
	public Stage stage;
	[TabGroup("main"), Tooltip("Optional stage description UI.")]
	public StageDescriptor stageDescriptor;

	[ShowInInspector, SerializeField]
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
	
	Animator _animator;
	Animator Animator
	{
		get
		{
			if (_animator) return _animator;
			_animator = GetComponent<Animator>();
			return _animator;
		}
	}

	public void EnterStage()
	{
		foreach (var e in _onStageEnter) e.Raise();
		stage?.Load(_stageLoadDelay);
	}

	public void SetSelected(bool selected)
	{
		Animator.SetBool("selected", selected);

		if (selected)
		{
			stageDescriptor?.Show();
			_onSelectedEvent.Invoke();
			foreach (var e in _onSelected) e.Raise();
		}
		else
		{
			stageDescriptor?.Hide();
			_onDeselectedEvent.Invoke();
			foreach (var e in _onDeselected) e.Raise();
		}
	}
}
