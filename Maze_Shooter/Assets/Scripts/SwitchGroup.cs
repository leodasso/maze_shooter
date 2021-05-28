using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SwitchGroup : MonoBehaviour
{
	[SerializeField]
	List<Switch> switches = new List<Switch>();

	[SerializeField, ]
	float timeBetweenSwitches = .5f;

	public bool reverse;

	[Button]
	void GetChildSwitches()
	{
		switches.Clear();
		switches = new List<Switch>();
		switches.AddRange(GetComponentsInChildren<Switch>());
	}

	[ButtonGroup]
	public void TurnGroupOn()
	{
		StartCoroutine(DoSwitchSequence(true));
	}

	[ButtonGroup]
	public void TurnGroupOff()
	{
		StartCoroutine(DoSwitchSequence(false));
	}

	IEnumerator DoSwitchSequence(bool willBeOn) 
	{
		if (reverse) {
			for (int i = switches.Count - 1; i >= 0; i--) 
			{
				switches[i].SwitchIsOn = willBeOn;
				yield return new WaitForSeconds(timeBetweenSwitches);
			}
		}
		else {
			for (int i = 0; i < switches.Count; i++) 
			{
				switches[i].SwitchIsOn = willBeOn;
				yield return new WaitForSeconds(timeBetweenSwitches);
			}
		}

	}
}
