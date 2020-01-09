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

    [Tooltip("Data unique to this devil. Allows for saving when I'm picked up by player."), TabGroup("Main")]
    public DevilData devilData;
    
    [Tooltip("Layers that this hazard will do damage to"), TabGroup("Main")]
    public LayerMask layersToDamage;
    
    [TabGroup("Main")] 
    public new Rigidbody rigidbody;
    
    [TabGroup("Main")]
    public new Collider collider;

    [TabGroup("Main")]
    public DevilState devilState = DevilState.Grounded;

    [TabGroup("Main")] 
    public FloatReference reboundSpeed;
    
    [TabGroup("Main")] 
    public FloatReference reboundRandomness;

    [TabGroup("Main")] 
    public FloatReference reboundVerticalness;
    
    [TabGroup("Events")]
    public UnityEvent onLaunch;
    
    [TabGroup("Events")]
    public UnityEvent onRebound;
    
    [Tooltip("Triggered when the player grabs the rebound out of mid-air.")]
    [TabGroup("Events")]
    public UnityEvent onReboundGrab;

    [Tooltip("Triggered when hits the ground after being launched.")]
    [TabGroup("Events")]
    public UnityEvent onHitGround;
    
    [Tooltip("Triggered when the player picks this devil up off the ground.")]
    [TabGroup("Events")]
    public UnityEvent onPickedUp;

    [Tooltip("Triggered when I deal damage to the thing I hit.")]
    [TabGroup("Events")]
    public UnityEvent onSuccessfulAttack;

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
        if (devilData)
        {
            // If the devil is out in the wild and marked as picked up in the save file,
            // we just want to disable it.
            if (devilState != DevilState.Following && devilData.IsRecruited())
            {
                gameObject.SetActive(false);
            }
        }
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

    void Rebound(Collision collision)
    {
        devilState = DevilState.Rebound;
        // Change the rebounded bullets to 'phantom props' so they can collide with player & be picked up,
        // but won't collide with other bullets
        gameObject.layer = LayerMask.NameToLayer("PhantomProps");
        
        // Set rebound velocity 
        var contact = collision.contacts[0];
        var newVelocity = contact.normal * reboundSpeed.Value;
        var randomVelocity = Random.onUnitSphere * reboundRandomness.Value;
        rigidbody.velocity = newVelocity + randomVelocity + Vector3.up * reboundVerticalness.Value;
        
        onRebound.Invoke();
    }

    public void ReturnToLauncher(DevilLauncher launcher)
    {
        gameObject.layer = LayerMask.NameToLayer("PlayerBullets");
        rigidbody.isKinematic = true;
        devilState = DevilState.Following;
        launcher.PickUpDevil(this);
        onPickedUp.Invoke();
    }

    /// <summary>
    /// Saves this devil as recruited by the player.
    /// </summary>
    void Recruit()
    {
        if (!devilData) return;
        if (devilData.IsRecruited()) return;
        
        // Set value in the save file that this is recruited
        devilData.Save(true);
        
        // TODO maybe special animation or effects for being recruited?
    }
    
    protected override void OnCollisionAction(Collision collision, Collider otherCol)
    {        
        // the devil can be caught on the rebound mid-air by the player's main collider for bonuses.
        DevilLauncher launcher = otherCol.GetComponent<DevilLauncher>();
        if (launcher)
        {
            // if this is a new touch, save the devil as picked up
            if (devilData) Recruit();

            ReturnToLauncher(launcher);
            onReboundGrab.Invoke();
            return;
        }
        
        DoDamage(collision, otherCol);
        
        if (devilState == DevilState.Launched)
            Rebound(collision);
        
        // check if hit ground, and if so, stick / set state!
        if (otherCol.gameObject.CompareTag("Ground"))
        {
            onHitGround.Invoke();
            StickInGround();
        }
    }


    float SymmetricRandom(float input)
    {
        return Random.Range(-input, input);
    }

    void StickInGround()
    {
        // Stick
        rigidbody.velocity = Vector3.zero;
        rigidbody.isKinematic = true;
        
        // Make sure pick upable
        devilState = DevilState.Grounded;
        
        // TODO animation for stuck in ground?
    }

    void DoDamage(Collision collision, Collider other)
    {
        if (!enabled) return;
        if (!Math.LayerMaskContainsLayer(layersToDamage, other.gameObject.layer)) return;
        if (devilState != DevilState.Launched) return;
        
        IDestructible destructible = other.GetComponent<IDestructible>();
        if (destructible == null) return;
        onSuccessfulAttack.Invoke();
        destructible.DoDamage(Damage, collision.GetContact(0).point, collision.GetContact(0).normal);

    }
}
