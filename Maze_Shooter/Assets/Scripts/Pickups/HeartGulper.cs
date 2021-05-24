using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartGulper : PickupGulper
{
	public Health health;

	protected override void OnTriggerEnter(Collider other)
	{
		TryGulpPickup<HeartPickup>(other);
	}

	protected override void OnGulp<T>(T pickup)
	{
		HeartPickup heart = pickup as HeartPickup;

		// if player has full hp? TODO
		if (health.HpIsFull) {
			Debug.Log("Want to heal " + health.name + " but its HP is full. maybe spawn coins?");
			return;
		}

		// otherwise add to player HP
		health.Heal(heart.amount);
	}
}