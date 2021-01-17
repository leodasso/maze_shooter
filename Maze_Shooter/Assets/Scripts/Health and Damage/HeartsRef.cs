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

	public Hearts constantValue;

	public HeartsValue valueObject;

	bool isGlobal => useConstant == PropertyType.Global;
	
	public Hearts Value
	{
		get { return useConstant == PropertyType.Local ? constantValue : valueObject.Value; }

		set {
			if ( useConstant == PropertyType.Global) valueObject.Value = value;
			else constantValue = value;
		}
	}
}
    
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(HeartsRef))]
public class HeartsRefDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		// Using BeginProperty / EndProperty on the parent property means that
		// prefab override logic works on the entire property.
		EditorGUI.BeginProperty(position, label, property);

		// Draw label
		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

		// Don't make child fields be indented
		var indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		float remainingWidth = position.width - EditorGlobals.propTypeEnumWidth;

		// Calculate rects
		var enumRect = new Rect(position.x, position.y, EditorGlobals.propTypeEnumWidth, position.height);
		var valueRect = new Rect(position.x + EditorGlobals.propTypeEnumWidth + 5, position.y, remainingWidth - 8, position.height);
		
		// get the enum index. 0 = local, 1 = global
		SerializedProperty enumProperty = property.FindPropertyRelative("useConstant");
		int enumIndex = enumProperty.enumValueIndex;

		// Draw fields - passs GUIContent.none to each so they are drawn without labels
		EditorGUI.PropertyField(enumRect, property.FindPropertyRelative("useConstant"), GUIContent.none);
		if (enumIndex == 0)
			EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("constantValue"), GUIContent.none);
		if (enumIndex == 1)
			EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("valueObject"), GUIContent.none);

		// Set indent back to what it was
		EditorGUI.indentLevel = indent;
		EditorGUI.EndProperty();
	}
}
#endif
