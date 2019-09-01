using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using Sirenix.OdinInspector;

public class Projectile : MonoBehaviour
{
	public FloatReference speed;
	public FloatReference lifetime;

	[ToggleGroup("addForceOverLifetime")]
	public bool addForceOverLifetime;
	[ToggleGroup("addForceOverLifetime")]
	public Vector2 forceOverLifetime;

	
	
	Gun _whoFiredMe;
	float _lifetimeTimer;
	Vector2 _velocity;

	void OnEnable()
	{
		_lifetimeTimer = 0;
	}

	// Update is called once per frame
	void Update () 
	{
		transform.Translate(0, speed.Value * Time.deltaTime, 0, Space.Self);
		

		if (addForceOverLifetime)
		{
			_velocity += forceOverLifetime * Time.deltaTime;
			transform.Translate(_velocity * Time.deltaTime, Space.World);
		}

		_lifetimeTimer += Time.deltaTime;
		if (_lifetimeTimer >= lifetime.Value)
			Destroy(gameObject);
	}
}
