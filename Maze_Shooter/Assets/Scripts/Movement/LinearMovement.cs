using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;

public class LinearMovement : MonoBehaviour
{
    public FloatReference linearSpeed;
    public float speedMultiplier = 1;
    public Vector2 initVector;

    [Tooltip("If I collide with objects of these layers, I will reverse direction")]
    public LayerMask layersThatChangeDirection;

    protected Rigidbody2D _rigidbody2D;

    Vector2 _currentVector;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _currentVector = initVector;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (!Math.LayerMaskContainsLayer(layersThatChangeDirection, other.gameObject.layer)) return;
        
        Vector2 newVector = Vector2.Reflect(_currentVector, other.contacts[0].normal);
        Debug.DrawRay(other.contacts[0].point, newVector.normalized, Color.magenta, 30);

        _currentVector = newVector;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_rigidbody2D) return;

        Vector2 force = _currentVector.normalized * speedMultiplier * linearSpeed.Value;
        _rigidbody2D.AddForce(force);
    }
}
