using System.Collections;
using System.Collections.Generic;
using Arachnid;
using Sirenix.OdinInspector;
using UnityEngine;

[TypeInfoBox("Destroys this gameobject after the lifetime")]
public class Lifetime : MonoBehaviour
{
    [Tooltip("This object's lifetime (in seconds)")]
    public FloatReference lifetime;
    
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(KillMe), lifetime.Value);
    }

    void KillMe()
    {
        Destroy(gameObject);
    }
}
