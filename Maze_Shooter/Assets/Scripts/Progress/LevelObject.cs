using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class LevelObject : MonoBehaviour
{
	[MinValue(0)]
	[Tooltip("The level at which this object awakens. For always awake, leave at 0")]
	public int minLevel = 0;

	[ReadOnly]
	public bool awake;
	public List<LevelVolume> volumes = new List<LevelVolume>();
	public UnityEvent onAwaken;
	public UnityEvent onSleep;

    // Start is called before the first frame update
    void Start()
    {
        CalculateIfAwake();
    }

	public void RefreshVolumesList() {
		volumes = volumes.Where(x => x != null).Where(x => x.isActiveAndEnabled == true).ToList();
		CalculateIfAwake();
	}


	void AddVolume(LevelVolume volume) {
		if (volumes.Contains(volume)) return;
		volumes.Add(volume);
		if (!awake) CalculateIfAwake();
	}

	void RemoveVolume(LevelVolume volume) {
		volumes.Remove(volume);
		if (awake) CalculateIfAwake();
	}

	void OnTriggerEnter(Collider other) {
		LevelVolume volume = other.GetComponent<LevelVolume>();
		if (!volume) return;
		AddVolume(volume);
	}

	void OnTriggerExit(Collider other) {
		LevelVolume volume = other.GetComponent<LevelVolume>();
		if (!volume) return;
		RemoveVolume(volume);
	}

	void CalculateIfAwake() {
		int level = volumes.Count;
		if (level >= minLevel) {
			awake = true;
			onAwaken.Invoke();
			return;
		}

		if (level < minLevel) {
			awake = false;
			onSleep.Invoke();
		}
	}
}