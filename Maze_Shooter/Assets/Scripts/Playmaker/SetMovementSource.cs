using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;
using ShootyGhost;

public class SetMovementSource : FsmStateAction
{
	[RequiredField]
	[CheckForComponent(typeof(SpriteAnimationPlayer))]
	public FsmOwnerDefault gameObject;
        
    SpriteAnimationPlayer animPlayer;

    public DirectionSourceType sourceType;
    public bool resetOnExit;
    DirectionSourceType _previousValue = DirectionSourceType.Custom;

    public override void Reset()
    {
        animPlayer = null;
    }
    

    public override void OnEnter()
    {
		var go = Fsm.GetOwnerDefaultTarget(gameObject);


		if (go==null)
		{
			Finish();
			return;
		}

		animPlayer = go.GetComponent<SpriteAnimationPlayer>();

        if (animPlayer)
        {
            _previousValue = animPlayer.direction.source;
            animPlayer.SetMovementSourceType(sourceType);
        }
        Finish();
    }
    

    public override void OnExit()
    {
        if (resetOnExit && animPlayer)
            animPlayer.SetMovementSourceType(_previousValue);
    }
}
