using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	public FloatReference speed;
	public FloatReference lifetime;
	public IntReference damage;

	Gun _whoFiredMe;
	float _lifetimeTimer;

	// Use this for initialization
	void Start () 
	{
		
	}

	void OnEnable()
	{
		_lifetimeTimer = 0;
	}

	// Update is called once per frame
	void Update () 
	{
		transform.Translate(0, speed.Value * Time.deltaTime, 0, Space.Self);
		_lifetimeTimer += Time.deltaTime;

		if (_lifetimeTimer >= lifetime.Value)
			Destroy(gameObject);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		IDestructible iDestructible = other.GetComponent<IDestructible>();
		iDestructible?.DoDamage(damage.Value, transform.position, transform.right);
		Destroy(gameObject);
	}
}
