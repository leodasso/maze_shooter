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

		// if player has full hp? 
		if (health.HpIsFull) {
			heart.BreakApart();
			return;
		}

		// otherwise add to player HP
		heart.AddHearts();
		health.Heal(heart.amount);
	}
}