using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Coin : MonoBehaviour
{
    public float destroyDelay = .5f;
    public UnityEvent onGrabbed;
    
    public int value = 1;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void Grab()
    {
        onGrabbed.Invoke();
        Destroy(gameObject, destroyDelay);
    }
}
