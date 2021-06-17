using UnityEngine;

namespace Arachnid
{
	[CreateAssetMenu(menuName ="Arachnid/Int Value")]
	public class IntValue : ValueAsset<int>
	{
		protected override string Prefix()
		{
			return "int_";
		}

		protected override bool CanSave()
		{
			return true;
		}

		protected override void ProcessValueChange(int newValue)
		{
		}

		protected override bool ValueHasChanged(int newValue) 
		{
			return newValue != myValue;
		}

		public void IterateValue (int amount)
		{
			Value += amount;
		}

		public void ResetValue()
		{
			Value = 0;
		}
	}
}