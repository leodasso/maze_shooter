using UnityEngine;
using UnityEngine.Events;

public class LevelVolume : MonoBehaviour
{
	public UnityEvent onDisableEvent;

	void OnDisable() {
		onDisableEvent.Invoke();
	}
}