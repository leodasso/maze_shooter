using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arachnid;

public class MovementBase : MonoBehaviour, IControllable
{
    public float speedMultiplier = 1;
    public FloatReference speed;
    
    protected Rigidbody2D _rigidbody2D;
    protected Vector2 _direction;
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _direction = new Vector2(1, 0);
    }

    /// <summary>
    /// Returns the direction the mover is pointed to. Similar to velocity, however even if velocity
    /// is 0, this will still return the direction it intends to go.
    /// </summary>
    public Vector2 GetIntendedDirection()
    {
        return _direction;
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
        _direction = input.normalized;
    }

    public virtual void ApplyRightStickInput(Vector2 input)
    { }

    public string Name()
    {
        return "linearMovement" + name;
    }
}
