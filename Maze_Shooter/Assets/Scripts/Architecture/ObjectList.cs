using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public abstract class ObjectList<T> : ScriptableObject
{
    [ShowInInspector]
    public List<T> objects;
}