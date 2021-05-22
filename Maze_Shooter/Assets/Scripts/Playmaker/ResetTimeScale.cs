using HutongGames.PlayMaker;

[ActionCategory(ActionCategory.Time)]
public class ResetTimeScale : FsmStateAction
{
	public override void OnEnter()
    {
		TimeController.ReturnTimeScale();        
        Finish();
    }
}
