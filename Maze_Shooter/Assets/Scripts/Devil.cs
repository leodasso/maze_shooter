using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;


public class Devil : MonoBehaviour
{
    [TabGroup("Events")]
    public UnityEvent onLaunch;
    
    [TabGroup("Events")]
    public UnityEvent onHitTarget;
    
    [Tooltip("Triggered when a wall/non-damage-able prop is hit")]
    [TabGroup("Events")]
    public UnityEvent onHitWall;

    [Tooltip("Triggered when hits the ground after being launched.")]
    [TabGroup("Events")]
    public UnityEvent onHitGround;
    
    [Space, TabGroup("Main")] 
    public new Rigidbody rigidbody;
    [TabGroup("Main")]
    public new Collider collider;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Launch(Vector3 launchVelocity)
    {
        collider.isTrigger = false;
        rigidbody.isKinematic = false;
        rigidbody.velocity = launchVelocity;
        onLaunch.Invoke();
    }
}
