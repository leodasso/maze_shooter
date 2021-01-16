using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Arachnid;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class HealthPoints
{
	[SerializeField]
	int fractions;

	[SerializeField]
	int hearts;

	int PointsPerHeart => GameMaster.FractionsPerHeart;

	int TotalPoints => hearts * PointsPerHeart + fractions;

	void Add(int points) 
	{
		int newTotal = points + TotalPoints;
		Recalculate(newTotal);
	}

	void Recalculate(int newPoints) 
	{
		fractions = newPoints % PointsPerHeart;
		hearts = Mathf.FloorToInt(newPoints / PointsPerHeart);
	}

	public static HealthPoints operator+(HealthPoints left, HealthPoints right) 
	{
		HealthPoints newHP = new HealthPoints();
		newHP.Recalculate(left.TotalPoints + right.TotalPoints);
		return newHP;
	}

	public static HealthPoints operator-(HealthPoints left, HealthPoints right) 
	{
		HealthPoints newHP = new HealthPoints();
		int newPoints = left.TotalPoints - right.TotalPoints;
		if (newPoints < 0) newPoints = 0;
		newHP.Recalculate(newPoints);
		return newHP;
	}
}

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(HealthPoints))]
public class HealthPointsDrawer : UnityEditor.PropertyDrawer
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
		var heartsRect = new Rect(position.x, position.y, EditorGlobals.propTypeEnumWidth, position.height);
		var pointsRect = new Rect(position.x + EditorGlobals.propTypeEnumWidth + 5, position.y, remainingWidth - 8, position.height);
		

		// Draw fields - passs GUIContent.none to each so they are drawn without labels
		EditorGUI.PropertyField(heartsRect, property.FindPropertyRelative("hearts"), GUIContent.none);
		EditorGUI.PropertyField(pointsRect, property.FindPropertyRelative("fractions"), GUIContent.none);

		// Set indent back to what it was
		EditorGUI.indentLevel = indent;
		EditorGUI.EndProperty();
	}
}
#endif
