using Arachnid;
using UnityEngine;

public class PhysicsProjectile : Projectile
{
	public new Rigidbody rigidbody;

	public override void Fire(Vector3 dir)
	{
		base.Fire(dir);
		rigidbody.velocity = fireDirection * speed.Value;
	}
}
