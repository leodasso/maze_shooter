using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(GuidGenerator))]
public class KeySlot : MonoBehaviour
{
    public GuidGenerator guidGenerator;
    public GameObject keyPlacementParent;

    public UnityEvent onKeyInserted;
    bool _filled;

    // This function has a gameobject param for key and the other one has a 'Key' param,
    // I know that seems weird, but this one is kept to the generic 'gameobject' type so
    // it can be invoked with unity events. The other one is only called directly from the 
    // Key class.
    public void PlaceKey(GameObject keyObject)
    {
        if (_filled)
        {
            Debug.LogWarning("key slot " + name + " is already filled, but key " + keyObject.name + " is requesting to be " +
                             "placed in it.", gameObject);
            return;
        }
        
        Key key = keyObject.GetComponent<Key>();
        if (!key) return;

        _filled = true;
        key.PlaceInSlotForFirstTime(this);
        PlaceKeyInstantly(key);
        onKeyInserted.Invoke();
    }

    public void PlaceKeyInstantly(Key key)
    {
        key.transform.parent = keyPlacementParent.transform;
        key.transform.localPosition = Vector3.zero;
        key.onPlacedInSlot.Invoke();
    }
}
