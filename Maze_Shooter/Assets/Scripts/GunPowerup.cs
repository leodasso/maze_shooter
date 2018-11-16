using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using Sirenix.OdinInspector;

public class GunPowerup : MonoBehaviour
{
	[ToggleLeft, Tooltip("Set to true if this should just be a general level up for any gun the player has equipped")]
	public bool anyGun;
	[HideIf("anyGun")]
	public GunData gun;

	public List<Collection> triggerers = new List<Collection>();

	void OnTriggerEnter2D(Collider2D other)
	{
		CollectionElement c = other.GetComponent<CollectionElement>();
		if (c == null) return;
		
		if (!triggerers.Contains(c.collection)) return;

		foreach (var g in other.gameObject.GetComponentsInChildren<Gun>())
			LevelUpGun(g);
		
		Destroy(gameObject);
	}

	void LevelUpGun(Gun g)
	{
		if (anyGun)
		{
			g.Level++;
			return;
		}

		if (g.gunData == gun) g.Level++;
		else g.gunData = gun;
	}
}