using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Arachnid;

[CreateAssetMenu(menuName ="Arachnid/Hearts Value")]
public class HeartsValue : ScriptableObject
{
	[ToggleLeft]
	public bool readOnly;
	[SerializeField, ShowInInspector, OnValueChanged("RaiseEvents")]
	HealthPoints myValue;
	
	public HealthPoints Value
	{
		get { return myValue; }
		set {
			// If the value is changing, raise the onValueChange events
			if (value != myValue)
			{
				if (readOnly)
				{
					Debug.LogWarning(name + " value can't be set because it's readonly.", this);
					return;
				}
				myValue = value;
				RaiseEvents();
			}
		}
	}

	[AssetsOnly]
	public List<GameEvent> onValueChange;
	[MultiLineProperty(5)]
	public string comments;

	void RaiseEvents() {
		if (!Application.isPlaying) return;
		foreach (var e in onValueChange) e.Raise();
	}
}
