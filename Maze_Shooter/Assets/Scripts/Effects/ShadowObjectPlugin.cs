using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShadowObjectPlugin : MonoBehaviour
{
    public ShadowObject shadowObject;

    void Awake()
    {
        shadowObject.AddPlugin(this);
    }

    public abstract void Recalculate(float normalizedDist);
}
