using UnityEngine;

namespace Arachnid
{
    [CreateAssetMenu(menuName ="Arachnid/Object Value")]
    public class ObjectValue : ValueAsset<ScriptableObject>
    {
		protected override string Prefix()=> "obj_";
		protected override bool CanSave() => false;

		protected override void ProcessValueChange(ScriptableObject newValue)
		{		}

		protected override bool ValueHasChanged(ScriptableObject newValue)
		{
			return newValue != myValue;
		}		
    }
}