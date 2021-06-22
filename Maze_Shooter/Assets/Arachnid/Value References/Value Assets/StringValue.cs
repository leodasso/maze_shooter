using UnityEngine;

namespace Arachnid
{
    [CreateAssetMenu(menuName ="Arachnid/String Value")]
    public class StringValue : ValueAsset<string>
    {
		protected override string Prefix() => "string_";
		protected override bool CanSave() => true;

		// intentionally blank
		protected override void ProcessValueChange(string newValue)
		{
		}

		protected override bool ValueHasChanged(string newValue)
		{
			return newValue != myValue;
		}		
    }
}