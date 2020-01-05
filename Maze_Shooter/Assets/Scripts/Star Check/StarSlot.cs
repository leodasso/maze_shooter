using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSlot : MonoBehaviour
{
    [Tooltip("Stars needed to activate this light. Should be kept to easy numbers like 1, 10, 50.")]
    public int stars = 1;

    [Tooltip("Is this slot already active? If so, it won't need any stars to activate.")]
    public bool lightActive;
        
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void TryActivate()
    {
        if (lightActive) return;
        // TODO pull star(s) from the player
    }
}
