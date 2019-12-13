using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class Devil : MonoBehaviour
{
    public enum DevilState
    {
        Launched,
        Rebound,
        Grounded,
        
    }
    
    [Space, TabGroup("Main")] 
    public new Rigidbody rigidbody;
    
    [TabGroup("Main")]
    public new Collider collider;

    [TabGroup("Main"), ReadOnly]
    public DevilState devilState = DevilState.Grounded;
    
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
    
    [Tooltip("Triggered when the player picks this devil up off the ground.")]
    [TabGroup("Events")]
    public UnityEvent onPickedUp;
    

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Launch(Vector3 launchVelocity)
    {
        collider.isTrigger = false;
        rigidbody.isKinematic = false;
        rigidbody.velocity = launchVelocity;
        devilState = DevilState.Launched;
        onLaunch.Invoke();
    }

    void Rebound()
    {
        Debug.Log("Rebounded!", gameObject);
        devilState = DevilState.Rebound;
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    void OnCollisionEnter(Collision other)
    {
        if (devilState == DevilState.Launched)
            Rebound();
                
        DevilLauncher launcher = other.collider.GetComponent<DevilLauncher>();
        if (launcher)
        {
            ReturnToLauncher(launcher);
            return;
        }
        
        // TODO have return/bounce force randomized?
        
        // TODO if hit enemy, call onHitTarget
    }

    void ReturnToLauncher(DevilLauncher launcher)
    {
        gameObject.layer = LayerMask.NameToLayer("PlayerBullets");
        rigidbody.isKinematic = true;
        launcher.PickUpDevil(this);
        onPickedUp.Invoke();

    }
}
