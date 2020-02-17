using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GuidGenerator))]
public class KeySlot : MonoBehaviour
{
    public GuidGenerator guidGenerator;
    public GameObject keyPlacementParent;
    bool _filled;

    // This function has a gameobject param for key and the other one has a 'Key' param,
    // I know that seems weird, but this one is kept to the generic 'gameobject' type so
    // it can be invoked with unity events. The other one is only called directly from the 
    // Key class.
    public void PlaceKey(GameObject keyObject)
    {
        Debug.Log("Placing key!", keyObject);
        if (_filled) return;
        Key key = keyObject.GetComponent<Key>();
        if (!key) return;
        key.PlaceInSlotForFirstTime(this);
        PlaceKeyInstantly(key);
    }

    public void PlaceKeyInstantly(Key key)
    {
        if (_filled)
        {
            Debug.LogWarning("key slot " + name + " is already filled, but key " + key.name + " is requesting to be " +
                             "placed in it.", gameObject);
            return;
        }
        
        key.transform.parent = keyPlacementParent.transform;
        key.transform.localPosition = Vector3.zero;
        _filled = true;
        key.onPlacedInSlot.Invoke();
    }
}
