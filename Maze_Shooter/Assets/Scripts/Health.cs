﻿using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using Sirenix.OdinInspector;

public class Health : MonoBehaviour, IDestructible
{
	public IntReference hitPoints;
	
	[ShowInInspector, ReadOnly]
	int _currentHealth;

	// Use this for initialization
	void Start ()
	{
		_currentHealth = hitPoints.Value;
	}

	public void DoDamage(int amount, Vector2 pos, Vector2 dir)
	{
		_currentHealth -= amount;
		if (_currentHealth <= 0) Destruct();
	}

	public void Destruct()
	{
		Destroy(gameObject);
	}
}
