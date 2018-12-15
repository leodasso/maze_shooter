using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using UnityEngine;

public class DisplayDialog : FsmStateAction
{
    public Dialog dialog;
    [RequiredField]
    public GameObject speaker;

    public override void Reset()
    {
        dialog = null;
        speaker = null;
    }

    public override void OnEnter()
    {
        if (dialog != null)
        {
            dialog.Display(speaker);
        }
        
        Finish();
    }
}
