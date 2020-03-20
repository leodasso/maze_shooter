using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arachnid;

public class MovementBase : MonoBehaviour, IControllable
{
    public float speedMultiplier = 1;
    public FloatReference speed;
    
    [Tooltip("Optional - will set the current path tangent to the sprite animation player")]
    public SpriteAnimationPlayer spriteAnimationPlayer;
    
    protected Rigidbody _rigidbody;
    
    /// <summary>
    /// The intended direction of movement. Differs from velocity in that if there's a wall or blocker and
    /// velocity is 0, this will still show the intended direction.
    /// </summary>
    protected Vector3 direction;
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        direction = Vector3.zero;
    }

    public Vector3 GetDirection()
    {
        return direction;
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
        direction = Math.Project2Dto3D(Vector2.ClampMagnitude(input, 1));
    }

    public virtual void ApplyRightStickInput(Vector2 input) { }

    public void DoActionAlpha() { }

    public string Name()
    {
        return GetType().ToString() + " " + name;
    }
}
