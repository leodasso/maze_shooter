using UnityEngine;

namespace Arachnid
{
	[CreateAssetMenu(menuName ="Arachnid/Bool Value")]
	public class BoolValue : ValueAsset<bool>
	{
		protected override string Prefix() => "bool_";
		protected override bool CanSave() => true;

		protected override void ProcessValueChange(bool newValue)
		{
		}

		protected override bool ValueHasChanged(bool newValue) 
		{
			return newValue != myValue;
		}
	}
}