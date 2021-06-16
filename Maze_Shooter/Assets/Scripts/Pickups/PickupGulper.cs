using UnityEngine;
using UnityEngine.Events;
using Arachnid;
using Sirenix.OdinInspector;


public abstract class PickupGulper : MonoBehaviour
{
	protected abstract void OnTriggerEnter(Collider other);

	// Failsafe to prevent from saving to file when no value loaded
	protected bool savedValueLoaded = false;

	[SerializeField, PropertyOrder(300)]
	UnityEvent onTooFullToGulp;

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

		if (IsFull()) {
			otherPickup.GulpAttemptedButFull();
			onTooFullToGulp.Invoke();
			return;
		}

		otherPickup.GetGulped();
		OnGulp<T>(otherPickup);
	}

	protected abstract void OnGulp<T>(T pickup) where T : Pickup;



	/// <summary>
	/// Confirms that there is an asset in the project folder to store this value in during gameplay.
	/// Note that this is not part of the save file!
	/// </summary>
	/// <returns>True if value asset is correctly referenced</returns>
	protected bool ConfirmValueExistence<T> (ValueAsset<T> valueAsset)
	{
		if (valueAsset == null) {
			Debug.LogError(name + " has no value asset referenced to store its value!", gameObject);
			enabled = false;
			return false;
		}
		return true;
	}

	protected void LoadSavedValue<T>(ValueAsset<T> valueAsset, SavedProperty<T> valueSaveFile) 
	{
		valueAsset.Value = valueSaveFile.GetValue();
		savedValueLoaded = true;
	}

	protected void TrySave<T>(ValueAsset<T> valueAsset, SavedProperty<T> valueSaveFile)
    {
        if (!savedValueLoaded || ! valueAsset || !valueSaveFile) {
			Debug.LogError(name + " failed to save because it was missing the value asset or save file asset.");
			return;
		}

		valueSaveFile.Save(valueAsset.Value);
    }

	protected abstract bool IsFull();
}