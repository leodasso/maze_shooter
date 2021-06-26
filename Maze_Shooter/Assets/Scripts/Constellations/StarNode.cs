using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using Arachnid;

public class StarNode : MonoBehaviour
{
	[SerializeField]
	StarDataDictionary starDatas;

	[SerializeField]
	UnityEvent onSlotFilled;

	[SerializeField]
	GuidGenerator guidGenerator;


	void Start()
	{
		if (!guidGenerator) {
			Debug.LogError(name + " has no GUID generator! This will cause a critical failure with loading save data.", gameObject);
			enabled = false;
			return;
		}

		// if so, put in the filled visuals
	}

	public StarData MyStar() 
	{
		// TODO
		return null;
	}

	public void Fill(StarData star) 
	{
		// TODO fill with this star data

		onSlotFilled.Invoke();
	}
}
