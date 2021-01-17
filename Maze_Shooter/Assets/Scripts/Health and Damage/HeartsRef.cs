using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arachnid;
using PropertyType = Arachnid.PropertyType;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
#endif

[System.Serializable]
public class HeartsRef 
{
	public PropertyType useConstant = PropertyType.Local;

	public HealthPoints constantValue;

	public HeartsValue valueObject;

	bool isGlobal => useConstant == PropertyType.Global;
	
	public HealthPoints Value
	{
		get { return useConstant == PropertyType.Local ? constantValue : valueObject.Value; }

		set {
			if ( useConstant == PropertyType.Global) valueObject.Value = value;
			else constantValue = value;
		}
	}
}
    
#if UNITY_EDITOR
public class HeartsRefDrawer : OdinValueDrawer<HeartsRef>
{
	protected override void DrawPropertyLayout(GUIContent label)
	{
		HeartsRef value = this.ValueEntry.SmartValue;

		var rect = EditorGUILayout.GetControlRect();

		// In Odin, labels are optional and can be null, so we have to account for that.
		if (label != null)
		{
			rect = EditorGUI.PrefixLabel(rect, label);
		}

		var prev = EditorGUIUtility.labelWidth;
		EditorGUIUtility.labelWidth = 15;

		value.useConstant = (PropertyType)EditorGUI.EnumPopup(rect.AlignLeft(EditorGlobals.propTypeEnumWidth), value.useConstant);
		var rightRect = rect.AlignRight(rect.width - EditorGlobals.propTypeEnumWidth);

		if (value.useConstant == Arachnid.PropertyType.Local) 
			HealthPointsDrawer.DrawEditor(ref value.constantValue, rightRect);

		if (value.useConstant == Arachnid.PropertyType.Global) {
			value.valueObject = (HeartsValue)EditorGUI.ObjectField(rightRect, value.valueObject, typeof(HeartsValue));
		}

		EditorGUIUtility.labelWidth = prev;

		this.ValueEntry.SmartValue = value;
	}
}
#endif
