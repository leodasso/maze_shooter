using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Health))]
public class InvulnerableFlash : MonoBehaviour
{
	public List<Renderer> flashingRenderers = new List<Renderer>();
	public FloatReference flashTime;
	Health _health;

	bool _showing;
	float _flashTimer;

	[Button]
	void GetAllRenderers()
	{
		flashingRenderers.Clear();
		flashingRenderers.AddRange(GetComponentsInChildren<Renderer>());
	}

	// Use this for initialization
	void Start ()
	{
		_health = GetComponent<Health>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!_health) return;
		if (!_health.IsInvulnerable)
		{
			if (!_showing)
			{
				_showing = true;
				SetRenderers(true);
			};
			return;
		}

		_flashTimer += Time.deltaTime;
		if (!(_flashTimer >= flashTime.Value)) return;
		ToggleRenderers();
		_flashTimer = 0;
	}

	void ToggleRenderers()
	{
		_showing = !_showing;
		SetRenderers(_showing);
	}

	void SetRenderers(bool showing)
	{
		foreach (var r in flashingRenderers)
		{
			if (!r) continue;
			r.enabled = showing;
		}
	}
}