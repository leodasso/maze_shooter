using HutongGames.PlayMaker;

public class SaveBool : FsmStateAction
{
	public SavedBool savedBool;
	public bool newSavedValue;

	public override void Reset()
	{
		savedBool = null;
	}

	public override void OnEnter()
    {
		if (savedBool) {
			savedBool.runtimeValue = newSavedValue;
		}
		Finish();
    }
}
