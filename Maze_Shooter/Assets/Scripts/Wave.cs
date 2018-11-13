using System.Collections;
using System.Collections.Generic;
using Arachnid;
using Sirenix.OdinInspector;
using UnityEngine;

[TypeInfoBox("Used as a parent to a group of enemies. Helpful for grouping enemies into a sequence of waves.")]
public class Wave : MonoBehaviour
{
	[ToggleLeft]
	public bool disableChildrenOnAwake = true;
	
	[Tooltip("The index of this particular wave")]
	public int waveNumber;
	
	[Tooltip("Ref to the value of the current wave")]
	public IntValue waveCounter;

	[ToggleLeft, Tooltip("Hide all waves but this"), OnValueChanged("SetSolo")]
	public bool solo;
	
	[ToggleLeft]
	public bool debug;

	bool _consumed;

	void Awake()
	{
		if (disableChildrenOnAwake)
			SetChildrenActive(false);
	}

	public void CheckIfShouldActivate()
	{
		if (debug) 
			Debug.Log(name + "/" + waveNumber + " is checking if it should activate.");
		
		if (_consumed) return;
		if (waveCounter == null)
		{
			Debug.LogWarning(name + " is missing a waveCounter, it looks like maybe that's an accident?");
			return;
		}

		if (waveCounter.Value == waveNumber)
		{
			if (debug) Debug.Log(name + " is activating because it's index matches the waveNumbers, which is " + waveCounter.Value);
			SetChildrenActive(true);
			_consumed = true;
		}
	}

	void SetChildrenActive(bool active)
	{
		foreach (Transform child in transform)
		{
			child.gameObject.SetActive(active);
		}
	}

	void SetSolo()
	{
		List<Wave> waves = new List<Wave>();
		waves.AddRange(Resources.FindObjectsOfTypeAll<Wave>());
		
		if (solo)
		{
			foreach (var w in waves)
			{
				if (w==this) continue;
				w.gameObject.SetActive(false);
			}
		}
		else
		{
			foreach (var w in waves)
				w.gameObject.SetActive(true);
		}
	}
}
