using System.Collections;
using System.Collections.Generic;
using Arachnid;
using Sirenix.OdinInspector;
using UnityEngine;

[TypeInfoBox("Moves along a path with physics.")]
public class PathMovement : MovementBase
{    
	public CharacterPath path;
    
    protected override void FixedUpdate()
    {
		if (!path) 
		return;
		/*
        if (target && useTargetFinder) 
            direction = (target.transform.position - transform.position).normalized;
		*/
        
		base.FixedUpdate();
    }
}
