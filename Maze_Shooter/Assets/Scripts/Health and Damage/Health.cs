using System;
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

	[TabGroup("main"), Tooltip("(optional) events will be called on mainHealth as if it is damaged/killed")]
	public Health mainHealth;

	[DrawWithUnity, TabGroup("events")]
	public UnityEvent onDamagedEvent;

	[DrawWithUnity, TabGroup("events")]
	public UnityEvent onKilledEvent;

	public Action<int> onDamaged;
	public Action<int> onHealed;

	float _invulnerableTimer;

	public bool IsInvulnerable => _invulnerableTimer > 0;
	public bool IsKilled => _isKilled;
	public Hearts ActualHp {
		get {
			return mainHealth == null ? currentHp.Value : mainHealth.currentHp.Value;
		}
		set {
			if (mainHealth) mainHealth.currentHp.Value = value;
			else currentHp.Value = value;
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

		if (invulnerableTime.Value > 0)
			_invulnerableTimer = invulnerableTime.Value;
		
		onDamaged?.Invoke(ActualHp.TotalPoints);
		onDamagedEvent.Invoke();

		// invoke events from the main health component
		if (mainHealth) mainHealth.onDamagedEvent.Invoke();
	}

	public void Heal(int amount)
	{
		if (!enabled) return;
		ActualHp += amount;
		ActualHp = Mathf.Clamp(ActualHp.TotalPoints, 0, maxHearts.Value.TotalPoints);
		if (onHealed != null) onHealed.Invoke(amount);
	}

	public void SetHp(Hearts newHp)
	{
		if (!enabled) return;
		currentHp.Value = newHp;
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