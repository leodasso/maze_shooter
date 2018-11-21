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
	
	[TabGroup("events")]
	public List<GameEvent> onSelected = new List<GameEvent>();
	[TabGroup("events")]
	public List<GameEvent> onDeselected = new List<GameEvent>();
	
	[TabGroup("events")]
	public UnityEvent onSelectedEvent;
	[TabGroup("events")]
	public UnityEvent onDeselectedEvent;
	
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

	public void SetSelected(bool selected)
	{
		Animator.SetBool("selected", selected);

		if (selected)
		{
			stageDescriptor?.Show();
			onSelectedEvent.Invoke();
			foreach (var e in onSelected) e.Raise();
		}
		else
		{
			stageDescriptor?.Hide();
			onDeselectedEvent.Invoke();
			foreach (var e in onDeselected) e.Raise();
		}
	}
}
