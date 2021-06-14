using UnityEngine;
using Sirenix.OdinInspector;

namespace Arachnid
{
    [System.Serializable]
    public abstract class ValueRef<T>
    {
        [HorizontalGroup, LabelText("Value"), LabelWidth(60)]
        public PropertyType useConstant = PropertyType.Local;
        
        [HideIf("isGlobal"), HorizontalGroup, HideLabel]
        public T constantValue;

        [AssetsOnly, ShowIf("isGlobal"), HorizontalGroup, HideLabel]
        public ValueAsset<T> valueObject;

        bool isGlobal => useConstant == PropertyType.Global;
        
        public T Value
        {
            get
            {
                if (!valueObject) 
					return constantValue;
                return useConstant == PropertyType.Local ? constantValue : valueObject.Value;
            }

            set {
                if ( useConstant == PropertyType.Global) 
					valueObject.Value = value;
                else 
					constantValue = value;
            }
        }
    }
}