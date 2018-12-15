using System.Collections;
using System.Collections.Generic;
using Arachnid;
using HutongGames.PlayMaker;
using UnityEngine;

public class InvokeGameEventListener : FsmStateAction
{
    public GameEventListener listener;

    public override void Reset()
    {
        listener = null;
    }

    public override void OnEnter()
    {
        if (listener)
            listener.OnEventRaised();
        
        Finish();
    }
}
