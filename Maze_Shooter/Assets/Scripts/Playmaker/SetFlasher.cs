using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;

public class SetFlasher : FsmStateAction
{
    public Flasher flasher;

    [HutongGames.PlayMaker.Tooltip("Set the given flasher to be flashing or not.")]
    public bool setFlashing;

    public bool resetOnExit;

    public override void Reset()
    {
        flasher = null;
    }
    

    public override void OnEnter()
    {
        if (flasher)
            flasher.SetFlashing(setFlashing);
        
        Finish();
    }
    

    public override void OnExit()
    {
        if (resetOnExit)
            flasher.SetFlashing(!flasher.GetFlashing());
    }
}
