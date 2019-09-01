using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Arachnid;

public class GunZone : MonoBehaviour 
{
	public GunData gunData;

	public List<Collection> triggerers = new List<Collection>();

	void OnTriggerEnter2D(Collider2D other)
	{
		CollectionElement c = other.GetComponent<CollectionElement>();
		if (c == null) return;
		if (!triggerers.Contains(c.collection)) return;

		foreach (var g in other.gameObject.GetComponentsInChildren<Gun>())
			g.AddOverride(gunData);
	}

	void OnTriggerExit2D(Collider2D other)
	{
		CollectionElement c = other.GetComponent<CollectionElement>();
		if (c == null) return;
		if (!triggerers.Contains(c.collection)) return;

		foreach (var g in other.gameObject.GetComponentsInChildren<Gun>())
			g.RemoveOverride(gunData);
	}
}