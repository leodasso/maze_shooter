using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class Switch : MonoBehaviour
{
	[ShowInInspector, ToggleLeft, PropertyOrder(-200)]
	public bool SwitchIsOn {
		get {
			return isOn;
		}
		set {

			if (oneOff && beenSwitched) return;

			// ignore switching to same state
			if (isOn == value) return;
			isOn = value;
			beenSwitched = true;

			if (isOn) onTurnedOn.Invoke();
			if (!isOn) onTurnedOff.Invoke();

			onSwitched.Invoke(isOn);

			if (playMaker) {
				playMaker.SendEvent( isOn ? "turnOn" : "turnOff");
			}
		}
	}

	[ToggleLeft, Tooltip("Only allow this to switch one time.")]
	public bool oneOff;

	[Space, SerializeField, Tooltip("Sends events 'turnOn' and 'turnOff'")]
	PlayMakerFSM playMaker;


	[Space, SerializeField]
	UnityEvent onTurnedOn;

	[SerializeField]
	UnityEvent onTurnedOff;

	[SerializeField]
	UnityEvent<bool> onSwitched;

	bool beenSwitched = false;
	

	[SerializeField, HideInInspector]
	bool isOn;

	// below functions are like this for easy interface with playmaker
	public void TurnOn()
	{
		SwitchIsOn = true;
	}

	public void turnOff()
	{
		SwitchIsOn = false;
	}
}