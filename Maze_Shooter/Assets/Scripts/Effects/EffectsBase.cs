using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EffectsBase : MonoBehaviour
{
	[AssetsOnly, AssetList(Path = "Prefabs/Effects/")]
	public GameObject effectPrefab;

	[Tooltip("How long the instantiated effect will last until it's destroyed")]
	public float lifetime = 5;

	[Tooltip("Delay between the triggering event and the actual spawn of the effect (in seconds)")]
	public float delay = 0;

	static GameObject _effectsParent;
	static GameObject _newInstance;

	public static GameObject EffectsParent()
	{
		if (_effectsParent == null)
			_effectsParent = new GameObject("Effects");

		return _effectsParent;
	}

	protected float _lifetime;

	void Update()
	{
		_lifetime += Time.deltaTime;
	}

	protected void InstantiateEffect()
	{
		InstantiateEffect(transform.position);
	}

	protected virtual void InstantiateEffect(Vector3 position)
	{
		if (!Application.isPlaying) return;
		if (Time.unscaledTime < Mathf.Epsilon) return;
		
		// If this is being disabled because we're loading a scene, we don't want it to create an effect.
		if (GameMaster.transitioning) return;
		
		if (effectPrefab == null)
		{
			Debug.LogWarning("Effect prefab is not set for " + name, gameObject);
			return;
		}

		if (delay > Mathf.Epsilon)
			StartCoroutine(DelayedInstantiate());
		
		else
			Destroy(Instantiate(
				effectPrefab, position, transform.rotation, EffectsParent().transform), lifetime);
	}

	IEnumerator DelayedInstantiate()
	{
		yield return new WaitForSeconds(delay);
		if (!Application.isPlaying) yield break;
		Destroy(Instantiate(
			effectPrefab, transform.position, transform.rotation, EffectsParent().transform), lifetime);
	}
}