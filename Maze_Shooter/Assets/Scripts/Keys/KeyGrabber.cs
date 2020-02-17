using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyGrabber : MonoBehaviour
{
    public OrbiterGroup orbiterGroup;
    
    public void GrabKey(GameObject newKey)
    {
        Key keyComponent = newKey.GetComponent<Key>();
        if (!keyComponent) return;
        keyComponent.Acquire();
        
        Orbiter newOrbiter = newKey.GetComponent<Orbiter>();
        if (newOrbiter) orbiterGroup.AddOrbiter(newOrbiter);
    }
}
