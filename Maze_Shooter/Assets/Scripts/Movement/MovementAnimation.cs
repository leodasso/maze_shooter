using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arachnid;

public class MovementAnimation : MonoBehaviour
{
    [Tooltip("Animator to send speed and movement angle to")]
    public Animator animator;
    [Tooltip("This doesn't affect movement! Just offsets the movement angle that gets sent to the animator.")]
    public float movementAngleOffset = 45;
    public Vector2 moveInput;
    Rigidbody2D _rigidbody2D;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Send parameters to animator based on movement
        if (animator)
        {
            if (_rigidbody2D)
                animator.SetFloat("speed", _rigidbody2D.velocity.magnitude);
			
            animator.SetFloat("facingAngle", Math.AngleFromVector2(moveInput, movementAngleOffset));
        }

    }
}
