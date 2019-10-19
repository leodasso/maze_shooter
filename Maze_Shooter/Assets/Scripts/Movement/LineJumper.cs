using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class LineJumper : MovementBase
{
    [TabGroup("main")]
    public CurveObject jumpSpeed;
    
    [TabGroup("main")]
    public CurveObject jumpHeight;
    
    [TabGroup("main")]
    public PseudoDepth pseudoDepth;

    [TabGroup("main")]
    public float climbingSpeed = 1;
    
    [Tooltip("Optional: hazard component on this object. If linked, this will show a damage effect/animation when colliding" +
             " with an object of the layers that the hazard damages.")]
    [TabGroup("main")]
    public Hazard hazard;

    [TabGroup("main")]
    public LayerMask layersToGrab;

    [TabGroup("main"), Tooltip("Optional: object that will show the current jump direction")]
    public GameObject aimer; 
    
    [TabGroup("main")]
    public Vector2 aimDirection = Vector2.right;

    [TabGroup("events")]
    public UnityEvent onWallGrabbed;

    [TabGroup("events"), Tooltip("Triggered whenever I hit something that my hazard component damages. Obviously this only triggers" +
                                 " if there is a hazard component on this object.")]
    public UnityEvent onVictimHit;
    
    [TabGroup("events")]
    public UnityEvent onJumpBegin;

    [TabGroup("events")] 
    public UnityEvent onClimbComplete;

    // how long have been jumping for
    float _jumpTime = 0;
    bool _jumping = false;
    float startingHeight => jumpHeight ? jumpHeight.ValueFor(0) : 1;
    float _climbVelocity = 0;
    bool _climbing = false;

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + aimDirection.normalized * 5);
        if (aimer)
        {
            aimer.transform.eulerAngles = new Vector3(0, 0, Math.AngleFromVector2(aimDirection.normalized, 0));
        }
    }

    [ButtonGroup()]
    public void Jump()
    {
        if (!Application.isPlaying) return;
        _jumpTime = 0;
        _jumping = true;
        onJumpBegin.Invoke();
    }

    [ButtonGroup()]
    public void BeginClimbing()
    {
        if (_jumping)
        {
            Debug.Log(name + " currently jumping, can't climb!", gameObject);
            return;
        }

        _climbing = true;
        pseudoDepth.ApplyVelocity(climbingSpeed);
    }

    public void MirrorAim()
    {
        aimDirection = -aimDirection;
    }

    void EndClimb()
    {
        _climbing = false;
        pseudoDepth.ApplyVelocity(0);
        onClimbComplete.Invoke();
    }

    void Update()
    {
        if (aimer)
            aimer.transform.eulerAngles = new Vector3(0, 0, Math.AngleFromVector2(aimDirection.normalized, 0));

        if (_climbing)
            if (pseudoDepth.z >= startingHeight) EndClimb();
    }

    void FixedUpdate()
    {
        if (!_jumping) return;
        
        _rigidbody2D.velocity = jumpSpeed.ValueFor(_jumpTime) * speed.Value * speedMultiplier * aimDirection;
        if (pseudoDepth) pseudoDepth.z = jumpHeight.ValueFor(_jumpTime);
        _jumpTime += Time.fixedDeltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (Math.LayerMaskContainsLayer(layersToGrab, other.gameObject.layer))
        {
            if (_jumping && _jumpTime > .15f)
                WallGrabbed();
        }
        
        else if (hazard && Math.LayerMaskContainsLayer(hazard.layersToDamage, other.gameObject.layer))
        {
            onVictimHit.Invoke();
        }
    }

    void WallGrabbed()
    {
        onWallGrabbed.Invoke();
        _jumping = false;
        _jumpTime = 0;
        _rigidbody2D.velocity = Vector2.zero;
    }
}
