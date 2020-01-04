using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateLight : MonoBehaviour
{
    [Tooltip("Stars needed to activate this light. Should be kept to easy numbers like 1, 10, 50.")]
    public int stars = 1;

    [Tooltip("Is this light active? If all lights in a gate are active, the gate will activate.")]
    public bool lightActive;
        
    // Start is called before the first frame update
    void Start()
    {
        
    }
}
