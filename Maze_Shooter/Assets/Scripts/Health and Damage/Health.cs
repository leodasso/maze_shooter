using System;
using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDestructible
{
	[TabGroup("main")]
	public IntReference hitPoints;
	
	[ShowInInspector, DisplayAsString, Indent, TabGroup("main")]
	int _currentHealth;

	[Tooltip("How long after damaged will I be invulnerable?"), TabGroup("main")]
	public FloatReference invulnerableTime;
	
	[ToggleLeft, TabGroup("main")]
	public bool createDamageEffect;
	
	[AssetsOnly, AssetList(Path = "Prefabs/Effects/"), ShowIf("createDamageEffect"), Indent, TabGroup("main")]
	public GameObject damagedEffect;

	[TabGroup("main"), Tooltip("Destroy this gameobject when killed (HP reaches 0)"), ToggleLeft]
	public bool destroyWhenKilled = true;

	[ShowIf("createDamageEffect"), Indent, TabGroup("main")]
	public float damageEffectLifetime = 5;

	[DrawWithUnity, TabGroup("events")]
	public UnityEvent onDamagedEvent;

	[DrawWithUnity, TabGroup("events")]
	public UnityEvent onKilledEvent;

	public Action<int> onDamaged;
	public Action<int> onHealed;

	float _invulnerableTimer;

	public bool IsInvulnerable => _invulnerableTimer > 0;
	public bool IsKilled => _isKilled;
	public int CurrentHealth => _currentHealth;
	bool _isKilled;

	void Awake ()
	{
		_currentHealth = hitPoints.Value;
	}

	void Update()
	{
		if (_invulnerableTimer > 0)
			_invulnerableTimer -= Time.deltaTime;
	}

	public void DoDamage(int amount, Vector3 pos, Vector3 dir)
	{
		if (IsInvulnerable || !enabled) return;
		
		_currentHealth -= amount;
		if (_currentHealth <= 0)
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
		
		onDamaged?.Invoke(_currentHealth);
		onDamagedEvent.Invoke();
	}

	public void Heal(int amount)
	{
		if (!enabled) return;
		_currentHealth += amount;
		_currentHealth = Mathf.Clamp(_currentHealth, 0, hitPoints.Value);
		if (onHealed != null) onHealed.Invoke(amount);
	}

	public void SetHp(int newHp)
	{
		if (!enabled) return;
		_currentHealth = newHp;
	}

	public void Destruct()
	{
		_isKilled = true;
		onKilledEvent.Invoke();
		if (destroyWhenKilled) Destroy(gameObject);
	}
}