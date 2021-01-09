using System;
using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDestructible
{
	[TabGroup("main") ]
	public SavedInt savedMaxHp;

	[TabGroup("main"), LabelText("Max HP")]
	public IntReference hitPoints;
	
	[TabGroup("main"), LabelText("Current HP")]
	public IntReference currentHp;

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
	public int CurrentHealth => currentHp.Value;
	bool _isKilled;

	void Awake ()
	{
		if (savedMaxHp != null) hitPoints.Value = savedMaxHp.GetValue();
		currentHp.Value = hitPoints.Value;
	}

	void Update()
	{
		if (_invulnerableTimer > 0)
			_invulnerableTimer -= Time.deltaTime;
	}

	public void DoDamage(int amount, Vector3 pos, Vector3 dir)
	{
		if (IsInvulnerable || !enabled) return;
		
		currentHp.Value -= amount;
		if (currentHp.Value <= 0)
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
		
		onDamaged?.Invoke(currentHp.Value);
		onDamagedEvent.Invoke();
		if (mainHealth) mainHealth.onDamagedEvent.Invoke();
	}

	public void Heal(int amount)
	{
		if (!enabled) return;
		currentHp.Value += amount;
		currentHp.Value = Mathf.Clamp(currentHp.Value, 0, hitPoints.Value);
		if (onHealed != null) onHealed.Invoke(amount);
	}

	public void SetHp(int newHp)
	{
		if (!enabled) return;
		currentHp.Value = newHp;
	}

	public void Destruct()
	{
		_isKilled = true;
		onKilledEvent.Invoke();
		if (mainHealth) mainHealth.onKilledEvent.Invoke();
		if (destroyWhenKilled) Destroy(gameObject);
	}
}