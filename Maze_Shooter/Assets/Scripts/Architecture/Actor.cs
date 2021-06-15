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
    ActorPlaceholder _placeholderInstance;

    void Awake()
    {
        Deactivate();
    }

    public void Activate()
    {
		if (debug)
			Debug.Log(name + " is activating.", gameObject);

        gameObject.SetActive(true);
        culled = false;
        if (_placeholderInstance)
            _placeholderInstance.gameObject.SetActive(false);
    }

    public void Deactivate()
    {
		if (debug)
			Debug.Log(name + " is deactivating.", gameObject);

        if (!_placeholderInstance) InstantiatePlaceholder();
        _placeholderInstance.gameObject.SetActive(true);
        _placeholderInstance.transform.position = transform.position;
        culled = true;
        gameObject.SetActive(false);
    }

    void InstantiatePlaceholder()
    {
        _placeholderInstance = Instantiate(placeholderPrefab, transform.position, transform.rotation, transform.parent)
            .GetComponent<ActorPlaceholder>();
        _placeholderInstance.actor = this;
    }
}
