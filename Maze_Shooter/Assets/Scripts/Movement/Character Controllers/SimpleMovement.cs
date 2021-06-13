using System.Collections;
using System.Collections.Generic;
using Arachnid;
using Sirenix.OdinInspector;
using UnityEngine;

[TypeInfoBox("Moves towards a target with physics. To set a target, add a targetFinder component.")]
[AddComponentMenu("Character Controllers/Simple Movement")]
public class SimpleMovement : MovementBase
{    
	[ToggleLeft]
    public bool useTargetFinder = true;

	[ToggleLeft]
	public bool updateTargetDirectionEveryFrame = true;
    
    [Tooltip("(optional) Will just use whatever target the targetfinder has if this is set."), ShowIf("useTargetFinder")]
    public TargetFinder targetFinder;

	GameObject currentTarget => targetFinder ? targetFinder.currentTarget : null;
    
    protected override void FixedUpdate()
    {
        if (useTargetFinder && updateTargetDirectionEveryFrame) 
			UpdateDirectionToTarget();
        
		base.FixedUpdate();
    }

	public void UpdateDirectionToTarget()
	{
		if (currentTarget)
			direction = (currentTarget.transform.position - transform.position).normalized;
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
