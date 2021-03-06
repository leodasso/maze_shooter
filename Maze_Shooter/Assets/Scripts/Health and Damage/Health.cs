﻿using System;
using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using UnityEngine.Serialization;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDestructible
{
	[TabGroup("main"), LabelText("Max HP"), FormerlySerializedAs("hitPoints")]
	public HeartsRef maxHearts;
	
	[TabGroup("main"), LabelText("Current HP")]
	public HeartsRef currentHp;

	[ToggleLeft, TabGroup("main"), Tooltip("Set the current HP to max HP on start")]
	public bool setHpOnStart = true;

	[Tooltip("How long after damaged will I be invulnerable?"), TabGroup("main"), Space]
	public FloatReference invulnerableTime;
	
	[ToggleLeft, TabGroup("main")]
	public bool createDamageEffect;
	
	[AssetsOnly, AssetList(Path = "Prefabs/Effects/"), ShowIf("createDamageEffect"), Indent, TabGroup("main")]
	public GameObject damagedEffect;

	[TabGroup("main"), Tooltip("Destroy this gameobject when killed (HP reaches 0)"), ToggleLeft]
	public bool destroyWhenKilled = true;

	[ShowIf("createDamageEffect"), Indent, TabGroup("main")]
	public float damageEffectLifetime = 5;

	[TabGroup("main"), Tooltip("(optional) events will be called on mainHealth as if it is damaged/killed"), ReadOnly]
	public Health mainHealth;

	[DrawWithUnity, TabGroup("events")]
	public UnityEvent onDamagedEvent;

	[DrawWithUnity, TabGroup("events")]
	public UnityEvent onKilledEvent;

	public Action<int> onDamaged;
	public Action<int> onHealed;

	float _invulnerableTimer;

	public bool HpIsFull => ActualHp >= maxHearts.Value;
	public bool IsInvulnerable => _invulnerableTimer > 0;
	public bool IsKilled => _isKilled;
	public Hearts ActualHp {
		get {
			return currentHp.Value;
		}
		set {
			currentHp.Value = value;
		}
	} 

	float ActualInvulnerableTime {
		get {
			return mainHealth ? mainHealth.invulnerableTime.Value : invulnerableTime.Value;
		}
	}

	public float NormalizedHp => (float)ActualHp.TotalPoints / (float)maxHearts.Value.TotalPoints;


	bool _isKilled;

	void Awake ()
	{
		// if (savedMaxHp != null) maxHearts.Value = savedMaxHp.GetValue();
		if (setHpOnStart) ResetHp();
	}

	public void ResetHp() 
	{
		currentHp.Value = maxHearts.Value;
	}

	void Update()
	{
		if (_invulnerableTimer > 0)
			_invulnerableTimer -= Time.deltaTime;
	}

	[Button]
	public void DoDamage(int amount) 
	{
		Hearts newDamage = new Hearts();
		newDamage.hearts = amount;
		DoDamage(newDamage, transform.position + Vector3.back * .25f, Vector3.forward);
	}

	public void DoDamage(Hearts amount) 
	{
		DoDamage(amount, transform.position + Vector3.back * .25f, Vector3.forward);
	}

	public void DoDamage(Hearts amount, Vector3 pos, Vector3 dir)
	{
		if (IsInvulnerable || !enabled || IsKilled) return;
		if (mainHealth)
		{
			mainHealth.DoDamage(amount, pos, dir);
			return;
		}
		
		ActualHp -= amount;
		if (ActualHp.TotalPoints <= 0)
		{
			Destruct();
			return;
		}

		if (damagedEffect && createDamageEffect)
		{
			Quaternion effectRotation = Quaternion.LookRotation(dir);
			Destroy(Instantiate(damagedEffect, pos, effectRotation), damageEffectLifetime);
		}

		if (ActualInvulnerableTime > 0)
			_invulnerableTimer = ActualInvulnerableTime;
		
		onDamaged?.Invoke(ActualHp.TotalPoints);
		onDamagedEvent.Invoke();
	}

	public void Heal(Hearts amount)
	{
		if (!enabled) return;
		ActualHp += amount;
		ActualHp = Hearts.Clamp(ActualHp, 0, maxHearts.Value);
		if (onHealed != null) onHealed.Invoke(amount.TotalPoints);
	}

	public void SetHp(Hearts newHp)
	{
		if (!enabled) return;
		currentHp.Value = newHp;
	}

	public void HaunterSafeDestruct()
	{
		if (_isKilled) return;
		Debug.Log("HAunter safe destruct");
		_isKilled = true;
		onKilledEvent.Invoke();
		if (destroyWhenKilled) Destroy(gameObject);
	}

	public void Destruct()
	{
		if (_isKilled) return;
		_isKilled = true;
		onKilledEvent.Invoke();
		if (mainHealth) mainHealth.onKilledEvent.Invoke();
		if (destroyWhenKilled) Destroy(gameObject);
	}
}