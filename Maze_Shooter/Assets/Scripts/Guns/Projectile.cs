using Arachnid;
using UnityEngine;
using Sirenix.OdinInspector;

public abstract class Projectile: MonoBehaviour
{
	public FloatReference lifetime;
	Gun _whoFiredMe;
	protected float _lifetimeTimer;

	protected Vector3 fireDirection;

	protected virtual void OnEnable()
	{
		_lifetimeTimer = 0;
	}

	// Update is called once per frame
	protected virtual void Update ()
	{
		// Lifetime
		_lifetimeTimer += Time.deltaTime;
		if (_lifetimeTimer >= lifetime.Value)
			Destroy(gameObject);
	}

	public virtual void Fire(Vector3 dir)
	{
		fireDirection = dir;
	}
}