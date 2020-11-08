using System.Collections;
using System.Collections.Generic;
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
        
    }

	public void AddVolume(LevelVolume volume) {
		if (volumes.Contains(volume)) return;
		volumes.Add(volume);

		if (awake) return;
		int level = volumes.Count;
		if (level >= minLevel) {
			awake = true;
			onAwaken.Invoke();
		}
	}

	public void RemoveVolume(LevelVolume volume) {
		volumes.Remove(volume);

		if (!awake) return;
		int level = volumes.Count;
		if (level < minLevel) {
			awake = false;
			onSleep.Invoke();
		}
	}
}
