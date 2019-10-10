using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;

public class SetDepthVelocity : FsmStateAction
{
    [RequiredField]
    [CheckForComponent(typeof(PseudoDepth))]
    public FsmOwnerDefault gameObject;

    public float velocity;

    PseudoDepth _pseudoDepth;

    public override void Reset()
    {
        base.Reset();
        gameObject = null;
        _pseudoDepth = null;
    }

    public override void OnEnter()
    {
        var go = Fsm.GetOwnerDefaultTarget(gameObject);

        if (!go)
        {
            Finish();
            return;
        }

        _pseudoDepth = go.GetComponent<PseudoDepth>();

        if (!_pseudoDepth)
        {
            Finish();
            return;
        }
        
        _pseudoDepth.ApplyVelocity(velocity);
    }
}
