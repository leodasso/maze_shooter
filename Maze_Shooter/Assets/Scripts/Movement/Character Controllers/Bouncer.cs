using Arachnid;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class Bouncer : MonoBehaviour
{
    public float minBounceVelocity = 5;
    public float maxBounceVelocity = 7;
    public new Rigidbody rigidbody;

    [Tooltip("Only collisions with objects of these layers will have a bounce controlled by this component")]
    public LayerMask controlledBounceImpacts;

    [Tooltip("Apply a random horizontal velocity on bounce?")]
    public bool setHorizontalVelocityOnBounce;
    
    [ShowIf("setHorizontalVelocityOnBounce"), Indent(), LabelText("Magnitude")]
    [Tooltip("Max magnitude of the horizontal velocity applied on bounce. The vector is a random value within this magnitude")]
    public float horizontalVelocityMagnitude = 1;

    public UnityEvent onBounce;
   

    void OnCollisionEnter(Collision other)
    {
        if (Math.LayerMaskContainsLayer(controlledBounceImpacts, other.gameObject.layer))
            Bounced();
    }

    void Bounced()
    {
        // Calculate horizontal velocity
        Vector3 horizontalVel = setHorizontalVelocityOnBounce
            ? new Vector3(
                Random.Range(-horizontalVelocityMagnitude, horizontalVelocityMagnitude), 
                0,
                Random.Range(-horizontalVelocityMagnitude, horizontalVelocityMagnitude)
            )
            : Vector3.zero;

        // Calculate vertical velocity
        float yVelocity = Random.Range(minBounceVelocity, maxBounceVelocity);
        
        // Apply both to the rigidbody
        rigidbody.velocity = Vector3.up * yVelocity + horizontalVel;
        
        onBounce.Invoke();
    }
}
