using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Math = Arachnid.Math;

[TypeInfoBox("Fires things in the local Z forward axis")]
public class Gun : GunBase
{
	public bool limitAmmo;
	[ShowIf("limitAmmo")]
	public int maxAmmo;
	
	[ShowIf("limitAmmo")]
	public int currentAmmo;

	[Range(0, 1)]
	[Tooltip("Determines how intense the fire rate is. 0 is the lowest fire rate of the selected gun," +
	         " and 1 is the highest rate.")]
	public float fireRateIntensity = 1;	

	Vector2 FireRateRange => HasGunData ? GunData.firingRate : firingRate;
	float FireRate => Mathf.Lerp(FireRateRange.x, FireRateRange.y, fireRateIntensity);
	float Cooldown => 1f / FireRate;
	bool IsCoolingDown => _cooldownTimer < Cooldown;
	float RandomSpreadAngle => HasGunData ? Random.Range(-GunData.randomSpread, GunData.randomSpread) : 0;
	float _cooldownTimer;

	protected override void Start()
	{
		base.Start();
		_cooldownTimer = 0;
	}
	
	// Update is called once per frame
	protected override void Update ()
	{
		base.Update();

		if (!AllowFiring) return;
		
		if (_cooldownTimer <= Cooldown)
			_cooldownTimer += Time.deltaTime;
		else if (firing) Fire();
	}

	[Button]
	public void Fire()
	{
		if (IsCoolingDown || !enabled) return;
		if (!Ammo)
		{
			Debug.LogError("No ammo ref is set on gun " + name, gameObject);
			return;
		}

		CreateBullet(Vector2.zero, RandomSpreadAngle);
		_cooldownTimer = 0;
	}
}