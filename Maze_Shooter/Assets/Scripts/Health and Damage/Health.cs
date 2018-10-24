using System;
using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDestructible
{
	public IntReference hitPoints;
	
	[ShowInInspector, DisplayAsString, Indent()]
	int _currentHealth;

	[ToggleLeft]
	public bool createDamageEffect;
	
	[AssetsOnly, AssetList(Path = "Prefabs/Effects/"), ShowIf("createDamageEffect"), Indent()]
	public GameObject damagedEffect;

	[ShowIf("createDamageEffect"), Indent()]
	public float damageEffectLifetime = 5;

	[DrawWithUnity]
	public UnityEvent onDamagedEvent;

	[DrawWithUnity]
	public UnityEvent onKilledEvent;

	public Action<int> onDamaged;

	// Use this for initialization
	void Start ()
	{
		_currentHealth = hitPoints.Value;
	}

	public void DoDamage(int amount, Vector2 pos, Vector2 dir)
	{
		_currentHealth -= amount;
		if (_currentHealth <= 0) Destruct();
		else
		{
			if (damagedEffect && createDamageEffect)
			{
				Quaternion effectRotation = Quaternion.LookRotation(dir);
				Destroy(Instantiate(damagedEffect, pos, effectRotation), damageEffectLifetime);
			}
			
			onDamaged?.Invoke(_currentHealth);
			onDamagedEvent.Invoke();
		}
	}

	public void Destruct()
	{
		onKilledEvent.Invoke();
		Destroy(gameObject);
	}
}
