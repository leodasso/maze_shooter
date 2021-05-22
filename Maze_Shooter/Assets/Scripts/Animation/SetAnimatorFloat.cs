using UnityEngine;
using Sirenix.OdinInspector;

[TypeInfoBox("Uses the attached rigidbody2D to tell animator what the velocity is")]
public class SetAnimatorFloat : MonoBehaviour
{
    public Animator animator;
    public new Rigidbody2D rigidbody2D;

    [Tooltip("The name of the parameter that represent's the object's speed in the Animator")]
    public string floatName = "speed";

    // Update is called once per frame
    void Update()
    {
        if (!animator || !rigidbody2D) return;
        animator.SetFloat(floatName, rigidbody2D.velocity.magnitude);
    }
}