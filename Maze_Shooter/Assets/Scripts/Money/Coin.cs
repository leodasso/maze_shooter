using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Coin : MonoBehaviour
{
    public Vector3 spawnVelocity = new Vector3(0, 5, 0);
    public float spawnVelocityRandomness = 3;

    public new Rigidbody rigidbody;
    public float destroyDelay = .5f;
    public UnityEvent onGrabbed;
    
    public int value = 1;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody.velocity = spawnVelocity + Random.insideUnitSphere * spawnVelocityRandomness;
    }

    public void Grab()
    {
        onGrabbed.Invoke();
        Destroy(gameObject, destroyDelay);
    }
}
