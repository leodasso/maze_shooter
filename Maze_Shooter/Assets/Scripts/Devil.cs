using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using Arachnid;

public class Devil : ContactBase
{
    public enum DevilState
    {
        Launched,
        Rebound,
        Grounded,
        Following,
    }
    
    [Tooltip("Layers that this hazard will do damage to"), TabGroup("Main")]
    public LayerMask layersToDamage;
    
    [TabGroup("Main")] 
    public new Rigidbody rigidbody;
    
    [TabGroup("Main")]
    public new Collider collider;

    [TabGroup("Main")]
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

    public UnityEvent onReboundGrab;

    // TODO damage for special modes maybe?
    int Damage => 1;

    float _stillTimer = 0;

    public bool CanBePickedUp()
    {
        return devilState == DevilState.Grounded;
    }
    
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

    public void SetAsFollowing()
    {
        devilState = DevilState.Following;
    }

    void Rebound()
    {
        devilState = DevilState.Rebound;
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    public void ReturnToLauncher(DevilLauncher launcher)
    {
        gameObject.layer = LayerMask.NameToLayer("PlayerBullets");
        rigidbody.isKinematic = true;
        devilState = DevilState.Following;
        launcher.PickUpDevil(this);
        onPickedUp.Invoke();
    }
    
    protected override void OnCollisionAction(Collision collision, Collider otherCol)
    {
        if (devilState == DevilState.Launched)
            Rebound();
        
        // the devil can be caught on the rebound mid-air by the player's main collider for bonuses.
        DevilLauncher launcher = otherCol.GetComponent<DevilLauncher>();
        if (launcher)
        {
            ReturnToLauncher(launcher);
            onReboundGrab.Invoke();
            return;
        }
        
        // TODO check if hit ground, and if so, stick / set state!

        // TODO have return/bounce force randomized?

        // TODO if hit enemy, call onHitTarget, do damage
    }

    void DoDamage(Collision collision, Collider other)
    {
        if (!enabled) return;
        if (!Math.LayerMaskContainsLayer(layersToDamage, other.gameObject.layer)) return;
        if (devilState != DevilState.Launched) return;
        
        IDestructible destructible = other.GetComponent<IDestructible>();
        destructible?.DoDamage(Damage, collision.GetContact(0).point, collision.GetContact(0).normal);

    }
}
