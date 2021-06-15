using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Controls culling of creatures, dynamic props, anything that might be moving around.
/// Only the actors within view of the player should be active.
/// </summary>
public class Actor : MonoBehaviour
{
	[SerializeField, ToggleLeft]
	bool debug;
    public GameObject placeholderPrefab;
    
    [ShowInInspector, ReadOnly]
    public bool culled;

	public static HashSet<Actor> allActors = new HashSet<Actor>();

    void Awake()
    {
		allActors.Add(this);
    }

	void OnDestroy()
	{
		allActors.Remove(this);
	}

    public void Activate()
    {
		if (debug)
			Debug.Log(name + " is activating.", gameObject);

        gameObject.SetActive(true);
        culled = false;
    }

    public void Deactivate()
    {
		if (debug)
			Debug.Log(name + " is deactivating.", gameObject);

        culled = true;
        gameObject.SetActive(false);
    }
}