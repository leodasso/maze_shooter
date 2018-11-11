using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using Sirenix.OdinInspector;

public enum GunType {Player, Enemy}

public class Gun : MonoBehaviour
{
	[ToggleLeft]
	public bool firing;

	public FloatReference startFiringDelay;
	public FloatReference fireRate;
	[AssetsOnly]
	public FiringPattern firingPattern;
	[AssetsOnly, PreviewField, AssetList(AutoPopulate = false, Path = "Prefabs/Ammo")]
	public GameObject ammo;
	public GunType gunType;

	float Cooldown => 1f / fireRate.Value;
	float _cooldownTimer;
	float _startFiringTimer;

	// Use this for initialization
	void Start ()
	{
		_startFiringTimer = startFiringDelay.Value;
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
		
		if (_cooldownTimer >= 0) _cooldownTimer -= Time.deltaTime;
		
		else if (firing) Fire();
	}

	bool IsCoolingDown()
	{
		return _cooldownTimer > 0;
	}

	[Button]
	public void Fire()
	{
		if (IsCoolingDown()) return;
		
		if (!ammo)
		{
			Debug.LogError("No ammo ref is set on gun " + name, gameObject);
			return;
		}

		if (firingPattern)
			StartCoroutine(FireRoutine());
		else 
			CreateBullet(Vector2.zero, 0);

		_cooldownTimer = Cooldown;
	}

	IEnumerator FireRoutine()
	{
		float angle;
		float x;
		float y;
		float progress = 0;
			
		for (int i = 0; i < firingPattern.bullets; i++)
		{
			int switcher = i % 2 == 0 ? 1 : -1;
			progress = (float) i / (firingPattern.bullets - 1);
			x = Mathf.Lerp(0, firingPattern.widthSpread, progress) * switcher;
			y = Mathf.Lerp(0, firingPattern.heightSpread, progress);
			angle = Mathf.Lerp(0, firingPattern.angleSpread, progress) * switcher;

			CreateBullet(new Vector2(x, y), -Math.RoundToNearest(angle, firingPattern.snapAngle) );
			yield return new WaitForSeconds(firingPattern.interval);
		}
	}

	void CreateBullet(Vector2 offset, float angle)
	{
		Vector2 localOffset = transform.TransformPoint(offset);
		Debug.DrawLine(transform.position, localOffset, Color.yellow, 1);
		var newAmmo = Instantiate(ammo, localOffset, transform.rotation);
		newAmmo.transform.Rotate(0, 0, angle, Space.World);
		
		newAmmo.layer = LayerMask.NameToLayer(gunType == GunType.Enemy ? 
			"EnemyBullets" : "PlayerBullets");
	}
}
