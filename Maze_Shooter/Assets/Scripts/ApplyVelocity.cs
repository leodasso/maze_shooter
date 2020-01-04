using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ApplyVelocity : MonoBehaviour
{
    [Tooltip("Choose a min and max possible x velocity")]
    public Vector2 xVelocity;
    
    [Tooltip("Choose a min and max possible y velocity")]
    public Vector2 yVelocity;

    [Tooltip("Choose a min and max possible depth(height) velocity. Requires that there's a pseudo depth component.")]
    public Vector2 depthVelocity;

    [ToggleLeft, Tooltip("Picks a random velocity from the range and applies it to this object on start")]
    public bool applyVelocityOnStart;
    
    // Start is called before the first frame update
    void Start()
    {
        if (applyVelocityOnStart) SetMyVelocity();
    }

    [Button]
    public void SetMyVelocity()
    {
        Vector3 vel = new Vector3(
            Random.Range(xVelocity.x, xVelocity.y),
            Random.Range(yVelocity.x, yVelocity.y),
            Random.Range(depthVelocity.x, depthVelocity.y));
        
        SetVelocity(vel);
    }

    /// <summary>
    /// Applies a given velocity to this object. The Z axis is used to give 'height' velocity to the pseudo depth
    /// component. If there is no pseudo depth component, the Z axis is ignored.
    /// </summary>
    public void SetVelocity(Vector3 velocity)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        //PseudoDepth depth = GetComponent<PseudoDepth>();

        if (rb)
            rb.velocity = velocity;
        
        //if (depth)
        //    depth.ApplyVelocity(velocity.z);
    }
}
