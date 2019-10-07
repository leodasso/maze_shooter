using System;
using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using UnityEngine;

public class SetGunFireRate : FsmStateAction
{
    public Gun gun;
    [Range(0, 1), HutongGames.PlayMaker.Tooltip("Set the gun's fire rate intensity")]
    public float newIntensity = .5f;

    public bool resetOnExit;
    float _previousValue = -99;

    public override void Reset()
    {
        gun = null;
    }
    

    public override void OnEnter()
    {
        if (gun)
        {
            _previousValue = gun.fireRateIntensity;
            gun.fireRateIntensity = newIntensity;
        }
        Finish();
    }
    

    public override void OnExit()
    {
        if (resetOnExit && _previousValue >= 0)
            gun.fireRateIntensity = _previousValue;
    }
}
