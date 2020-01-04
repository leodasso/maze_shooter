using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;

public enum MovementMode
{
    Force = 0, Velocity = 1
}

public class LinearMovement : MovementBase
{
    [Tooltip("The initial direction I'll move. This is previewed as the yellow line coming from me.")]
    public Vector2 initVector;

    [Tooltip("If I collide with objects of these layers, I will reverse direction")]
    public LayerMask layersThatChangeDirection;

    public MovementMode movementMode = MovementMode.Force;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        direction = initVector.normalized;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)initVector.normalized * speedMultiplier * speed.Value);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (!Math.LayerMaskContainsLayer(layersThatChangeDirection, other.gameObject.layer)) return;
        
        Vector2 newVector = Vector2.Reflect(direction, other.contacts[0].normal);
        Debug.DrawRay(other.contacts[0].point, newVector.normalized, Color.magenta, 30);

        direction = newVector;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_rigidbody) return;

        Vector2 forceVector = direction.normalized * speedMultiplier * speed.Value;
        
        switch (movementMode)
        {
            case MovementMode.Force:
                _rigidbody.AddForce(forceVector);
                break;
            case MovementMode.Velocity:
                _rigidbody.velocity = forceVector;
                break;
        }
    }

    public override void ApplyLeftStickInput(Vector2 input)
    {
        if (input.magnitude > .5f)
            direction = input.normalized;
    }

}
