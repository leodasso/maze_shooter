using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Math = Arachnid.Math;

[TypeInfoBox("Fires things in the local Y axis")]
public class Gun : GunBase
{
	[Range(0, 1)]
	[Tooltip("Determines how intense the fire rate is. 0 is the lowest fire rate of the selected gun," +
	         " and 1 is the highest rate.")]
	public float fireRateIntensity = 1;
	
	FiringPattern FiringPattern {
		get
		{
			if (!HasGunData) return firingPattern;
			if (GunData.firingPatterns.Count < 1) return firingPattern;
			int maxAvailableFiringPattern = GunData.firingPatterns.Count - 1;
			int index = Mathf.Clamp(Level, 0, maxAvailableFiringPattern);
			return GunData.firingPatterns[index];
		}
	}
	
	Vector2 FireRateRange => HasGunData ? GunData.firingRate : firingRate;
	float FireRate => Mathf.Lerp(FireRateRange.x, FireRateRange.y, fireRateIntensity);
	float Cooldown => 1f / FireRate;
	bool IsCoolingDown => _cooldownTimer < Cooldown;
	float RandomSpreadAngle => HasGunData ? Random.Range(-GunData.firingSpread, GunData.firingSpread) : 0;
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

		if (FiringPattern)
			StartCoroutine(FireRoutine());
		else 
			CreateBullet(Vector2.zero, RandomSpreadAngle);

		_cooldownTimer = 0;
	}

	IEnumerator FireRoutine()
	{
		float angle;
		float x;
		float y;
		float progress = 0;
			
		for (int i = 0; i < FiringPattern.bullets; i++)
		{
			int switcher = i % 2 == 0 ? 1 : -1;
			progress = (float) i / Mathf.Max(1, FiringPattern.bullets - 1);
			
			x = Mathf.Lerp(0, FiringPattern.widthSpread, progress) * switcher;
			y = Mathf.Lerp(0, FiringPattern.heightSpread, progress);
			angle = Mathf.Lerp(0, FiringPattern.angleSpread, progress) * switcher;

			CreateBullet(new Vector2(x, y), -Math.RoundToNearest(angle, FiringPattern.snapAngle) + RandomSpreadAngle );
			yield return new WaitForSeconds(FiringPattern.interval);
		}
	}
}