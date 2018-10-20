using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using Sirenix.OdinInspector;

public enum GunType {Player, Enemy}

public class Gun : MonoBehaviour
{
	[ToggleLeft, System.NonSerialized, ShowInInspector, ReadOnly]
	public bool firing;
	public FloatReference fireRate;
	[AssetsOnly, PreviewField, AssetList(AutoPopulate = false, Path = "Prefabs/Ammo")]
	public GameObject ammo;
	public GunType gunType;

	float Cooldown => 1f / fireRate.Value;
	float _cooldownTimer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (_cooldownTimer >= 0) _cooldownTimer -= Time.deltaTime;
		
		else if (firing) Fire();
	}

	[Button]
	void Fire()
	{
		if (!ammo)
		{
			Debug.LogError("No ammo ref is set on gun " + name, gameObject);
			return;
		}

		var newAmmo = Instantiate(ammo, transform.position, transform.rotation);

		if (gunType == GunType.Enemy) newAmmo.layer = LayerMask.NameToLayer("EnemyBullets");
		else newAmmo.layer = LayerMask.NameToLayer("PlayerBullets");
		_cooldownTimer = Cooldown;
	}
}
