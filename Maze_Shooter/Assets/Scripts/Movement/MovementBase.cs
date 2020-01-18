using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arachnid;

public class MovementBase : MonoBehaviour, IControllable
{
    public float speedMultiplier = 1;
    public FloatReference speed;
    
    protected Rigidbody _rigidbody;
    
    /// <summary>
    /// The intended direction of movement. Differs from velocity in that if there's a wall or blocker and
    /// velocity is 0, this will still show the intended direction.
    /// </summary>
    protected Vector2 direction;
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        direction = new Vector2(0, 0);
    }

    /// <summary>
    /// Returns the direction the mover is pointed to. Similar to velocity, however even if velocity
    /// is 0, this will still return the direction it intends to go.
    /// </summary>
    public Vector3 GetIntendedDirection()
    {
        return Math.Project2Dto3D(direction);
    }

    public void IncreaseSpeedMultiplier(float amt)
    {
        speedMultiplier += amt;
    }

    public void SetSpeedMultiplier(float value)
    {
        speedMultiplier = value;
    }
    
    public virtual void ApplyLeftStickInput(Vector2 input)
    {
        direction = Vector2.ClampMagnitude(input, 1);
    }

    public virtual void ApplyRightStickInput(Vector2 input) { }

    public void DoActionAlpha() { }

    public string Name()
    {
        return "linearMovement" + name;
    }
}
