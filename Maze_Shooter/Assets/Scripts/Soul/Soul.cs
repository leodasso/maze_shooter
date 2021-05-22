using Arachnid;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using System.Collections.Generic;

[TypeInfoBox("Soul is nourished by the light. If a spirit strays too far from the light, " +
             "soul is drained. When soul is empty, the spirit will be dragged into hell. \n " + 
			 " this object should always be SoulLight layer! It checks for trigger enters of other soul lights.")]
public class Soul : MonoBehaviour
{
	[ToggleLeft, SerializeField]
	bool debug;

	[ShowInInspector, ReadOnly]
	bool isLit = false;

	[SerializeField, Space]
	UnityEvent onEnterDark;

	[SerializeField]
	UnityEvent onEnterLight;
    
    List<Collider> lightSources = new List<Collider>();

	Vector3 lastSafePos;

    // Update is called once per frame
    void Update()
    {
        bool litThisFrame = false;

        foreach (var soulLight in lightSources)
        {
            if (!soulLight.enabled) continue;
			if (!soulLight.gameObject.activeInHierarchy) continue;
			litThisFrame = true;
			break;
        }

		if (!isLit && litThisFrame) {
			isLit = true;
			onEnterLight.Invoke();
			if (debug)
				Debug.Log(name + " has entered the LIGHT");
		}

		else if (isLit && !litThisFrame) {
			isLit = false;
			onEnterDark.Invoke();
			if (debug)
				Debug.Log(name + " has entered the DARK");
		}

		// remember safe / lit position
		if (isLit) 
			lastSafePos = transform.position;
    }

	public void ReturnToSafePos() 
	{
		transform.position = lastSafePos;
	}

    public void Reset()
    {
        lightSources.Clear();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!lightSources.Contains(other))
            lightSources.Add(other);
    }

    void OnTriggerExit(Collider other)
    {
        lightSources.Remove(other);
    }
}