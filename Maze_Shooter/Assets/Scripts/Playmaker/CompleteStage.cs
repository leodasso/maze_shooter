using UnityEngine;
using HutongGames.PlayMaker;

public class CompleteStage : FsmStateAction
{
    public Stage stage;
    
    public override void Reset()
    {
        stage = null;
    }
    

    public override void OnEnter()
    {
        if (stage)
            stage.CompleteStage();
        Finish();
    }
}
