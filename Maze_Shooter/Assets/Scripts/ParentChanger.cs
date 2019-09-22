using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arachnid;
using Sirenix.OdinInspector;

public class ParentChanger : MonoBehaviour
{
    public List<Collection> thingsToParent;

    [Button]
    public void EncapsulateThings()
    {
        foreach (var collection in thingsToParent)
        {
            foreach (var element in collection.elements)
            {
                element.transform.parent = transform;
            }
        }
    }

    [Button]
    public void Unparent()
    {
        foreach (Transform t in transform)
        {
            t.parent = null;
        }
    }
}