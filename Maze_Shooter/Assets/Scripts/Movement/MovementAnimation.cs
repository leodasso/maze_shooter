using UnityEngine;
using Arachnid;

public class MovementAnimation : MonoBehaviour, IControllable
{
    [Tooltip("Animator to send speed and movement angle to")]
    public Animator animator;
    [Tooltip("This doesn't affect movement! Just offsets the movement angle that gets sent to the animator.")]
    public float movementAngleOffset = 45;
    Rigidbody _rigidbody;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void ApplyLeftStickInput(Vector2 input)
    {
        if (!animator) return;
        if (_rigidbody)
            animator.SetFloat("speed", _rigidbody.velocity.magnitude);
			
        animator.SetFloat("facingAngle", Math.AngleFromVector2(input, movementAngleOffset));
    }

    public void ApplyRightStickInput(Vector2 input)
    {}

    public string Name()
    {
        return "MovementAnimation" + name;
    }
}
