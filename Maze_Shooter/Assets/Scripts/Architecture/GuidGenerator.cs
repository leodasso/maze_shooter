using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GuidGenerator : MonoBehaviour
{
    /// <summary>
    /// List of all guid generators in the currently loaded scene
    /// </summary>
    static List<GuidGenerator> allInstances = new List<GuidGenerator>();
    
    public string uniqueId;

    /// <summary>
    /// Returns the game(o)bject with the given guidGenerator guid attached to it.
    /// If no gameObject is loaded with the given guid, returns null.
    /// </summary>
    public static GameObject GetObjectForGuid(string guid)
    {
        foreach (var guidGenerator in allInstances)
        {
            if (guidGenerator.uniqueId == guid) return guidGenerator.gameObject;
        }
        return null;
    }

    void Awake()
    {
        allInstances.Add(this);
    }

    [Button]
    void GenerateGuid()
    {
        Guid guid = Guid.NewGuid();
        uniqueId = guid.ToString();
    }
}
