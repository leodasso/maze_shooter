using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShootyGhost;


public class ProjectileShield : MonoBehaviour
{
	[SerializeField]
	SpriteAnimationPlayer animationPlayer;

	public void ProjectileHit(GameObject projectile)
	{
		Vector3 hitVector = transform.position - projectile.transform.position;
		Debug.DrawRay(transform.position, hitVector);

		animationPlayer.direction.customDirection = -hitVector;
		animationPlayer.direction.source = DirectionSourceType.Custom;
	}


}
