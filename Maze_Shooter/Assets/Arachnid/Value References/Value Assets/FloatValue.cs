using UnityEngine;

namespace Arachnid
{
    [CreateAssetMenu(menuName ="Arachnid/Float Value")]
    public class FloatValue : ValueAsset<float>
    {
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