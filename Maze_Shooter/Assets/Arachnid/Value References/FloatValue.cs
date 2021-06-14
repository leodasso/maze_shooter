using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Arachnid
{
    [CreateAssetMenu(menuName ="Arachnid/Float Value")]
    public class FloatValue : ValueAsset<float>
    {
		protected override bool ValueHasChanged(float newValue)
		{
			return System.Math.Abs(newValue - myValue) > Mathf.Epsilon;
		}		

        /// <summary>
        /// Increases the value by the given amount
        /// </summary>
        public void IterateValue (float amount)
        {
            Value += amount;
        }

        /// <summary>
        /// Resets the value back to zero.
        /// </summary>
        public void ResetValue()
        {
            Value = 0;
        }
    }
}