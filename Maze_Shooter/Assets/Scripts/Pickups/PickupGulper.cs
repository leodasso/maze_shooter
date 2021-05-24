using UnityEngine;

public abstract class PickupGulper : MonoBehaviour
{
	protected abstract void OnTriggerEnter(Collider other);

	/// <summary>
	/// Checks the type of the other pickup, and if it's a match, calls OnGulp with the other pickup.
	/// Also calls Pickup.GetGulped on the pickup instance.
	/// </summary>
	/// <param name="other">The other collider that has hit this trigger</param>
	/// <typeparam name="T">Type of pickups to accept</typeparam>
	protected void TryGulpPickup<T>(Collider other) where T : Pickup
	{
		T otherPickup = other.GetComponent<T>();
		if (!otherPickup) return;

		otherPickup.GetGulped();
		OnGulp<T>(otherPickup);
	}

	protected abstract void OnGulp<T>(T pickup) where T : Pickup;
}