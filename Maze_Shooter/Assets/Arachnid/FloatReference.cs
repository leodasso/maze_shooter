﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

#if UNITY_EDITOR

using Sirenix.OdinInspector.Editor;
using UnityEditor;
using Sirenix.Utilities;

#endif

namespace Arachnid
{
    [System.Serializable]
    public class FloatReference
    {
        [HorizontalGroup, LabelText("Value"), LabelWidth(60)]
        public PropertyType useConstant = PropertyType.Local;
        
        [HideIf("isGlobal"), HorizontalGroup, HideLabel]
        public float constantValue;
        [AssetsOnly, ShowIf("isGlobal"), HorizontalGroup, HideLabel]
        public FloatValue valueObject;

        bool isGlobal => useConstant == PropertyType.Global;
        
        public float Value
        {
            get { return useConstant == PropertyType.Local ? constantValue : valueObject.Value; }

            set {
                if ( useConstant == PropertyType.Global) valueObject.Value = value;
                else constantValue = value;
            }
        }
    }
    
#if UNITY_EDITOR

    /*
    public sealed class FloatRefDrawer : OdinValueDrawer<FloatReference>
    {
        protected override void DrawPropertyLayout(IPropertyValueEntry<FloatReference> entry, GUIContent label)
        {
            FloatReference value = entry.SmartValue;

            var rect = EditorGUILayout.GetControlRect();

            // In Odin, labels are optional and can be null, so we have to account for that.
            if (label != null)
            {
                rect = EditorGUI.PrefixLabel(rect, label);
            }

            var prev = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 0;

            value.useConstant = (PropertyType)EditorGUI.EnumPopup(rect.AlignLeft(rect.width * 0.4f), "", value.useConstant);

            Rect rightRect = rect.AlignRight(rect.width * 0.6f);

            if (value.useConstant == PropertyType.Global)
            {
                value.valueObject =
                    (FloatValue) EditorGUI.ObjectField(rightRect, "", value.valueObject, typeof(FloatValue), false);
            }
            else
            {
                value.constantValue = EditorGUI.FloatField(rightRect, value.constantValue);
            }

            EditorGUIUtility.labelWidth = prev;
            entry.SmartValue = value;
        }
    }
    */

#endif
}