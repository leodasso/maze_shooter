using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Math = Arachnid.Math;
using Arachnid;

[TypeInfoBox("Fires things in the local Z forward axis")]
public class Gun : GunBase
{
	public bool limitAmmo;
	[ShowIf("limitAmmo")]
	public IntReference maxAmmo;

	int currentAmmo;

	[Range(0, 1)]
	[Tooltip("Determines how intense the fire rate is. 0 is the lowest fire rate of the selected gun," +
	         " and 1 is the highest rate.")]
	public float fireRateIntensity = 1;	

	[Tooltip("Used to play sfx for this gun")]
	public AudioAction audioAction;

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
		currentAmmo = maxAmmo.Value;
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

		if (audioAction) {
			audioAction.audioCollection = HasAmmo ? gunData.fireSound : gunData.dryShotSound;
			audioAction.Play();
		}

		
		if (HasAmmo) {
			SpendAmmo();
			CreateBullet(Vector2.zero, RandomSpreadAngle);
		}

		_cooldownTimer = 0;
	}

	public override void Reload()
	{
		base.Reload();
		currentAmmo = maxAmmo.Value;
		audioAction.audioCollection = gunData.reloadSound;
		audioAction.Play();
	}

	bool HasAmmo => limitAmmo? currentAmmo > 0 : true;

	void SpendAmmo() 
	{
		if (!HasAmmo || !limitAmmo) return;
		currentAmmo--;
	}
}