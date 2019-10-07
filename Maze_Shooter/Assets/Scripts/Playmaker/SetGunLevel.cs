using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;

public class SetGunLevel : FsmStateAction
{
    public Gun gun;

    [HutongGames.PlayMaker.Tooltip("Increase the gun level by 1. This overrides the 'new level' var.")]
    public bool levelUp;

    [HutongGames.PlayMaker.Tooltip("Set the gun's new level")]
    public int newLevel = 1;

    public bool resetOnExit;
    int _previousValue = -99;

    public override void Reset()
    {
        gun = null;
    }
    

    public override void OnEnter()
    {
        if (gun)
        {
            _previousValue = gun.Level;

            if (levelUp) gun.Level++;
            else gun.Level = newLevel;
        }
        Finish();
    }
    

    public override void OnExit()
    {
        if (resetOnExit && _previousValue >= 0)
            gun.Level = _previousValue;
    }
}
