﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EffectsBase : MonoBehaviour
{
	[AssetsOnly, AssetList(Path = "Prefabs/Effects/")]
	public GameObject effectPrefab;

	[Tooltip("How long the instantiated effect will last until it's destroyed")]
	public float lifetime = 5;

	static GameObject _effectsParent;
	static GameObject _newInstance;

	protected float _lifetime;

	void Update()
	{
		_lifetime += Time.deltaTime;
	}

	protected virtual void InstantiateEffect()
	{
		if (Time.unscaledTime < Mathf.Epsilon)
		{
			return;
		}
		
		if (effectPrefab == null)
		{
			Debug.LogWarning("Effect prefab is not set for " + name, gameObject);
			return;
		}

		if (_effectsParent == null)
			_effectsParent = new GameObject("Effects");
		
		Destroy(Instantiate(
			effectPrefab, transform.position, transform.rotation, _effectsParent.transform), lifetime);
	}
}