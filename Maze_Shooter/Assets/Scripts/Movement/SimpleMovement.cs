using System.Collections;
using System.Collections.Generic;
using Arachnid;
using Sirenix.OdinInspector;
using UnityEngine;

[TypeInfoBox("Moves towards a target with physics. To set a target, add a targetFinder component.")]
public class SimpleMovement : MovementBase
{    
    [Tooltip("The thing I'll move towards. Keep in mind if there's a targetFinder referenced, it will overwrite" +
             " whatever you put in here.")]
    public GameObject target;

    public bool useTargetFinder = true;
    
    [Tooltip("(optional) Will just use whatever target the targetfinder has if this is set."), ShowIf("useTargetFinder")]
    public TargetFinder targetFinder;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        
        if (targetFinder && targetFinder.currentTarget && useTargetFinder) 
            target = targetFinder.currentTarget.gameObject;
    }
    
    void FixedUpdate()
    {
        if (target && useTargetFinder) 
            direction = (target.transform.position - transform.position).normalized;
        
        _rigidbody.AddForce(direction * speed.Value * speedMultiplier * Time.fixedDeltaTime);
    }

    // These functions are for playmaker to easily interface with this behavior
    public void DisableTargeting()
    {
        useTargetFinder = false;
    }

    public void EnableTargeting()
    {
        useTargetFinder = true;
    }
}
