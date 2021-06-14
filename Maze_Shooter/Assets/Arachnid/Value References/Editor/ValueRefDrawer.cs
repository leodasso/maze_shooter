using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;

namespace Arachnid
{
    public abstract class ValueRefDrawer<T> : PropertyDrawer
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

			// D
            if (enumIndex == 0)
                EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("constantValue"), GUIContent.none);

			if (enumIndex == 1) {
				// divide the rects
				float leftRectWidth = valueRect.width * .7f;
				var leftRect = new Rect(valueRect.x, valueRect.y, leftRectWidth, valueRect.height);
				var rightRect = new Rect(valueRect.x + leftRectWidth, valueRect.y, valueRect.width - leftRectWidth, valueRect.height);

				// ref to the asset in project folder that holds the value
				var globalObject = property.FindPropertyRelative("valueObject");

				// draw the property for the object ref
				EditorGUI.ObjectField(leftRect, globalObject, typeof(T), GUIContent.none);
				
				// draw the property for the actual value
				var objRefValue = globalObject.objectReferenceValue;
				if (objRefValue != null) {
					SerializedObject serializedObject = new SerializedObject(objRefValue);
					SerializedProperty value = serializedObject.FindProperty("myValue");
					EditorGUI.PropertyField(rightRect, value, GUIContent.none);
					serializedObject.ApplyModifiedProperties();
				}
			}

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}