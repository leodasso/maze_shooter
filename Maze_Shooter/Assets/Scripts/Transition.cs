using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
	public float duration = .5f;

	void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

	// Called by animation
	public void DestroyMe()
	{
		Destroy(gameObject);
	}

	public void InstantiateMe()
	{
		Instantiate(gameObject);
	}
}
