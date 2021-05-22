using Arachnid;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using System.Collections.Generic;

[TypeInfoBox("Soul is nourished by the light. If a spirit strays too far from the light, " +
             "soul is drained. When soul is empty, the spirit will be dragged into hell.")]
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
    
    List<SoulLight> lightSources = new List<SoulLight>();

	Vector3 lastSafePos;

    // Update is called once per frame
    void Update()
    {
        bool litThisFrame = false;

        foreach (SoulLight soulLight in lightSources)
        {
            if (!soulLight.isActiveAndEnabled) continue;
            if (soulLight.DoesLightPoint(transform.position)) {
				litThisFrame = true;
				break;
			}
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
        SoulLight soulLight = other.GetComponent<SoulLight>();
        if (!soulLight) return;
        
        if (!lightSources.Contains(soulLight))
            lightSources.Add(soulLight);
    }

    void OnTriggerExit(Collider other)
    {
        SoulLight soulLight = other.GetComponent<SoulLight>();
        if (!soulLight) return;

        lightSources.Remove(soulLight);
    }
}
