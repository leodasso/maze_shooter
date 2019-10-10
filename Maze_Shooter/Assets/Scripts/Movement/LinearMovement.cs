using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;

public class LinearMovement : MovementBase
{
    public Vector2 initVector;

    [Tooltip("If I collide with objects of these layers, I will reverse direction")]
    public LayerMask layersThatChangeDirection;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _direction = initVector.normalized;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (!Math.LayerMaskContainsLayer(layersThatChangeDirection, other.gameObject.layer)) return;
        
        Vector2 newVector = Vector2.Reflect(_direction, other.contacts[0].normal);
        Debug.DrawRay(other.contacts[0].point, newVector.normalized, Color.magenta, 30);

        _direction = newVector;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_rigidbody2D) return;

        Vector2 force = _direction.normalized * speedMultiplier * speed.Value;
        _rigidbody2D.AddForce(force);
    }
}
