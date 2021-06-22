using HutongGames.PlayMaker;
using Arachnid;

public class SaveBool : FsmStateAction
{
	public BoolValue savedBool;
	public bool newSavedValue;

	public override void Reset()
	{
		savedBool = null;
	}

	public override void OnEnter()
    {
		if (savedBool) {
			savedBool.Value = newSavedValue;
		}
		Finish();
    }
}
