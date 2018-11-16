using System.Collections;
using Arachnid;
using UnityEngine;
using Sirenix.OdinInspector;

public enum GunType {Player, Enemy}

public class Gun : MonoBehaviour
{
	[ToggleLeft]
	public bool firing;
	public FloatReference startFiringDelay;
	
	[Range(0, 1)]
	public float fireRateIntensity = 1;
	[MinValue(0)]
	public int level = 0;
	public GunType gunType;
	
	public GunData gunData;
	[HideIf("HasGunData"), BoxGroup("local gun")]
	[MinMaxSlider(.1f, 60, true), Tooltip("Number of shots per second. The minimum is when the player is barely touching joystick," +
	                                " and max is when they're at full tilt.")]
	public Vector2 firingRate;
	[AssetsOnly, HideIf("HasGunData"), BoxGroup("local gun")]
	public FiringPattern firingPattern;
	[AssetsOnly, PreviewField, AssetList(AutoPopulate = false, Path = "Prefabs/Ammo"), BoxGroup("local gun"), HideIf("HasGunData")]
	public GameObject ammo;
	

	FiringPattern FiringPattern {
		get
		{
			if (!HasGunData) return firingPattern;
			if (gunData.firingPatterns.Count < 1) return firingPattern;
			int maxAvailableFiringPattern = gunData.firingPatterns.Count - 1;
			int index = Mathf.Clamp(level, 0, maxAvailableFiringPattern);
			return gunData.firingPatterns[index];
		}
	}
	
	Vector2 FireRateRange => HasGunData ? gunData.firingRate : firingRate;
	float FireRate => Mathf.Lerp(FireRateRange.x, FireRateRange.y, fireRateIntensity);
	float Cooldown => 1f / FireRate;
	float _cooldownTimer;
	float _startFiringTimer;
	bool HasGunData => gunData != null;
	GameObject Ammo => HasGunData ? gunData.ammo : ammo;
	bool IsCoolingDown => _cooldownTimer < Cooldown;

	// Use this for initialization
	void Start ()
	{
		_startFiringTimer = startFiringDelay.Value;
		_cooldownTimer = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Starting delay
		if (_startFiringTimer > 0)
		{
			_startFiringTimer -= Time.deltaTime;
			return;
		}
		
		if (_cooldownTimer <= Cooldown)
			_cooldownTimer += Time.deltaTime;
		else if (firing) Fire();
	}

	[Button]
	public void Fire()
	{
		if (IsCoolingDown) return;
		if (!Ammo)
		{
			Debug.LogError("No ammo ref is set on gun " + name, gameObject);
			return;
		}

		if (FiringPattern)
			StartCoroutine(FireRoutine());
		else 
			CreateBullet(Vector2.zero, 0);

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

			CreateBullet(new Vector2(x, y), -Math.RoundToNearest(angle, FiringPattern.snapAngle) );
			yield return new WaitForSeconds(FiringPattern.interval);
		}
	}

	void CreateBullet(Vector2 offset, float angle)
	{
		Vector2 localOffset = transform.TransformPoint(offset);
		Debug.DrawLine(transform.position, localOffset, Color.yellow, 1);
		var newAmmo = Instantiate(Ammo, localOffset, transform.rotation);
		newAmmo.transform.Rotate(0, 0, angle, Space.World);
		
		newAmmo.layer = LayerMask.NameToLayer(gunType == GunType.Enemy ? 
			"EnemyBullets" : "PlayerBullets");
	}
}