using System.Collections;
using System.Collections.Generic;
using Arachnid;
using Sirenix.OdinInspector;
using UnityEngine;

[TypeInfoBox("Moves along a path with physics.")]
public class PathMovement : MovementBase
{    
	[Tooltip("Choose a path for this character to move along")]
	public CharacterPath path;

	[ShowIf("HasPath"), Tooltip("The index of the path point this character will start at.")]
	[OnValueChanged("ProcessStartingIndex"), PropertyRange(0, "MaxIndex")]
	public int startingIndex;

	bool HasPath => path != null;

	int MaxIndex => HasPath ? path.pathPoints.Count - 1 : 1;

	void ProcessStartingIndex()
	{
		if (!HasPath) return;
		startingIndex = Mathf.Clamp(startingIndex, 0, path.pathPoints.Count - 1);


	}
    
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
