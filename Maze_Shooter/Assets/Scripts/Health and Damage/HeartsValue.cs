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
	Hearts myValue;
	
	[ShowInInspector]
	public Hearts Value
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

				if (value > myValue)
					RaiseEvents(onValueIncrease);

				if (value < myValue)
					RaiseEvents(onValueDecrease);

				myValue = value;
				RaiseEvents(onValueChange);
			}
		}
	}

	[AssetsOnly, SerializeField]
	List<GameEvent> onValueChange;

	[AssetsOnly, SerializeField]
	List<GameEvent> onValueIncrease;

	[AssetsOnly, SerializeField]
	List<GameEvent> onValueDecrease;

	[MultiLineProperty(5)]
	public string comments;

	void RaiseEvents(List<GameEvent> eventList) 
	{
		if (!Application.isPlaying) return;
		foreach (var e in eventList) e.Raise();
	}
}
