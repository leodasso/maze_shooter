using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Sirenix.OdinInspector;

public class StarNode : MonoBehaviour
{
	
	[SerializeField]
	UnityEvent onSlotFilled;

	void Start()
	{
		// check if I have a saved star

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
