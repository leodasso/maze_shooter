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
		health.Heal(heart.amount);
	}

	protected override bool IsFull()
	{
		return health.HpIsFull;
	}
}