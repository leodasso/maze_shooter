using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Arachnid;

[CreateAssetMenu(menuName ="Arachnid/Hearts Value")]
public class HeartsValue : ValueAsset<Hearts>
{
	protected override string Prefix()
	{
		return "hearts_";
	}

	protected override bool CanSave()
	{
		return true;
	}

	protected override bool ValueHasChanged(Hearts newValue)
	{
		return newValue != myValue;
	}

	protected override void ProcessValueChange(Hearts newValue)
	{
		if (newValue > myValue)
			RaiseEvents(onValueIncrease);

		if (newValue < myValue)
			RaiseEvents(onValueDecrease);
	}


	[AssetsOnly, SerializeField]
	List<GameEvent> onValueIncrease;

	[AssetsOnly, SerializeField]
	List<GameEvent> onValueDecrease;

}
