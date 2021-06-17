using UnityEngine;

namespace Arachnid
{
    [CreateAssetMenu(menuName ="Arachnid/Float Value")]
    public class FloatValue : ValueAsset<float>
    {
		protected override string Prefix()
		{
			return "float_";
		}

		protected override bool CanSave()
		{
			return true;
		}

		protected override void ProcessValueChange(float newValue)
		{
			
		}

		protected override bool ValueHasChanged(float newValue)
		{
			return System.Math.Abs(newValue - myValue) > Mathf.Epsilon;
		}		

        public void IterateValue (float amount)
        {
            Value += amount;
        }

        public void ResetValue()
        {
            Value = 0;
        }
    }
}