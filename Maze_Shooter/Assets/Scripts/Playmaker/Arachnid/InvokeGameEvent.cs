using System.Collections;
using System.Collections.Generic;
using Arachnid;
using HutongGames.PlayMaker;
using UnityEngine;

public class InvokeGameEvent : FsmStateAction
{
	[RequiredField]
	public GameEvent gameEvent;

	public override void Reset()
	{
		gameEvent = null;
	}

	public override void OnEnter()
	{
		gameEvent.Raise();
		Finish();
	}
}
