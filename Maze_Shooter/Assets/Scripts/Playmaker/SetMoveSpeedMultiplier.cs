using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;

public class SetMoveSpeedMultiplier : FsmStateAction
{
    public MovementBase mover;

    [HutongGames.PlayMaker.Tooltip("Set the movement speed multiplier")]
    public float multiplier = 1;
    public bool resetOnExit;
    float _previousValue = -99;

    public override void Reset()
    {
        mover = null;
    }
    

    public override void OnEnter()
    {
        if (mover)
        {
            _previousValue = mover.speedMultiplier;
            mover.speedMultiplier = multiplier;
        }
        Finish();
    }
    

    public override void OnExit()
    {
        if (resetOnExit && _previousValue >= 0)
            mover.speedMultiplier = _previousValue;
    }
}
