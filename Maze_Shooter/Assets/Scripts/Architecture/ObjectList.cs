using System.Collections.Generic;
using Sirenix.OdinInspector;

public abstract class ObjectList<T> : SerializedScriptableObject
{
    [ShowInInspector]
    public Dictionary<string, T> objects;
}