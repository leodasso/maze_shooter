using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;

public class SetAnimatorGroupTrigger : FsmStateAction
{
    public AnimatorGroup animatorGroup;
    public string trigger;

    public override void Reset()
    {
        animatorGroup = null;
        trigger = null;
    }

    public override void OnEnter()
    {
        if (animatorGroup)
            animatorGroup.SetTrigger(trigger);
        
        Finish();
    }
}
