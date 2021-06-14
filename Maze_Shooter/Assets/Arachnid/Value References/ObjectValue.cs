using UnityEngine;

namespace Arachnid
{
    [CreateAssetMenu(menuName ="Arachnid/Object Value")]
    public class ObjectValue : ValueAsset<ScriptableObject>
    {
		protected override bool ValueHasChanged(ScriptableObject newValue)
		{
			return newValue != myValue;
		}		
    }
}