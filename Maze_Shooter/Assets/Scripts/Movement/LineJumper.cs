﻿using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class LineJumper : MovementBase
{
    [TabGroup("main")]
    public CurveObject jumpSpeed;
    
    [Tooltip("Optional: hazard component on this object. If linked, this will show a damage effect/animation when colliding" +
             " with an object of the layers that the hazard damages.")]
    [TabGroup("main")]
    public Hazard hazard;

    [TabGroup("main")]
    public LayerMask layersToGrab;

    [TabGroup("main"), Tooltip("Optional: object that will show the current jump direction")]
    public GameObject aimer; 

    [TabGroup("events")]
    public UnityEvent onWallGrabbed;

    [TabGroup("events"), Tooltip("Triggered whenever I hit something that my hazard component damages. Obviously this only triggers" +
                                 " if there is a hazard component on this object.")]
    public UnityEvent onVictimHit;
    
    [TabGroup("events")]
    public UnityEvent onJumpBegin;

    // how long have been jumping for
    float _jumpTime = 0;
    bool _jumping = false;
	Vector3 _aimVector;

    [ButtonGroup()]
    public void Jump()
    {
        if (!Application.isPlaying) return;
        _jumpTime = 0;
        _jumping = true;
        onJumpBegin.Invoke();
    }


    public void MirrorAim()
    {
        direction = -direction;
    }

	void GrabNearestWall() {
		List<Collider> grabbables = new List<Collider>();
		grabbables.AddRange(Physics.OverlapSphere(transform.position, 50, layersToGrab, QueryTriggerInteraction.Ignore));
		grabbables.OrderBy(x => Vector3.SqrMagnitude(transform.position - x.transform.position));
		Debug.Log("The nearest grabbable is " + grabbables[0].name, grabbables[0].gameObject);		
	}


    protected override void Update()
    {
		base.Update();
		if (direction.magnitude > .15f)
			_aimVector = direction;

		if (aimer)
			aimer.transform.eulerAngles = new Vector3(0, 0, Math.AngleFromVector2(new Vector2(_aimVector.x, _aimVector.z), 0));
    }

    void FixedUpdate()
    {
        if (!_jumping) return;
        
        _rigidbody.AddForce(jumpSpeed.ValueFor(_jumpTime) * movementProfile.movementForce * speedMultiplier * _aimVector.normalized * Time.fixedDeltaTime);
        _jumpTime += Time.fixedDeltaTime;
    }

    void OnTriggerEnter(Collider other)
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
        _rigidbody.velocity = Vector2.zero;
    }

	public override void DoActionAlpha() 
	{
		 Jump();
	}
}
