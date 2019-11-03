using UnityEngine;
using Arachnid;

public class MovementAnimation : MonoBehaviour, IControllable
{
    [Tooltip("Animator to send speed and movement angle to")]
    public Animator animator;
    [Tooltip("This doesn't affect movement! Just offsets the movement angle that gets sent to the animator.")]
    public float movementAngleOffset = 45;
    Rigidbody2D _rigidbody2D;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void ApplyLeftStickInput(Vector2 input)
    {
        if (!animator) return;
        if (_rigidbody2D)
            animator.SetFloat("speed", _rigidbody2D.velocity.magnitude);
			
        animator.SetFloat("facingAngle", Math.AngleFromVector2(input, movementAngleOffset));
    }

    public void ApplyRightStickInput(Vector2 input)
    {}

    public string Name()
    {
        return "MovementAnimation" + name;
    }
}
