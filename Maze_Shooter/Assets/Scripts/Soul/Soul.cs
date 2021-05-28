using Arachnid;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;

[TypeInfoBox("Soul is nourished by the light. If a spirit strays too far from the light, " +
             "soul is drained. When soul is empty, the spirit will be dragged into hell. \n " + 
			 " this object should always be SoulLight layer! It checks for trigger enters of other soul lights.")]
public class Soul : MonoBehaviour
{
	public GameObject soulOwner;

	[ToggleLeft, SerializeField]
	bool debug;

	[ShowInInspector, ReadOnly]
	bool isLit = false;

	[ShowInInspector, ReadOnly]
	bool inInterior = false;

	[SerializeField, Space]
	UnityEvent onEnterDark;

	[SerializeField]
	UnityEvent onEnterLight;
    
    List<Collider> lightSources = new List<Collider>();

	Vector3 lastSafePos;

    // Update is called once per frame
    void Update()
    {
        bool litThisFrame = inInterior;

		lightSources = lightSources.Where(x => x.enabled && x.gameObject.activeInHierarchy).ToList();

		litThisFrame = lightSources.Count > 0;

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

	public void EnterInterior() 
	{
		inInterior = true;
	}

	public void ExitInterior()
	{
		inInterior = false;
	}

	public void ReturnToSafePos() 
	{
		soulOwner.transform.position = lastSafePos;
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