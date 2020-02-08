using System;
using UnityEngine;
using Sirenix.OdinInspector;

public class GuidGenerator : MonoBehaviour
{
    public string uniqueId;
   
    [Button]
    void GenerateGuid()
    {
        Guid guid = Guid.NewGuid();
        uniqueId = guid.ToString();
    }
}
