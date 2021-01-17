using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arachnid;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
#endif

[System.Serializable]
public struct HealthPoints
{
	public int fractions;

	[MinValue(0)]
	public int hearts;

	public static int PointsPerHeart => GameMaster.FractionsPerHeart;

	public int TotalPoints => hearts * PointsPerHeart + fractions;

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

	public static HealthPoints operator+(HealthPoints left, int right) 
	{
		HealthPoints newHP = new HealthPoints();
		newHP.Recalculate(left.TotalPoints + right);
		return newHP;
	}

	public static HealthPoints operator-(HealthPoints left, int right) 
	{
		HealthPoints newHP = new HealthPoints();
		int newPoints = left.TotalPoints - right;
		if (newPoints < 0) newPoints = 0;
		newHP.Recalculate(newPoints);
		return newHP;
	}

	public static bool operator==(HealthPoints left, HealthPoints right) 
	{
		return left.TotalPoints == right.TotalPoints;
	}

	public static bool operator!=(HealthPoints left, HealthPoints right) 
	{
		return left.TotalPoints != right.TotalPoints;
	}

	public static implicit operator HealthPoints(int qty) {
		HealthPoints newHp = new HealthPoints();
		newHp.hearts = qty;
		newHp.fractions = 0;
		return newHp;
	}
}

#if UNITY_EDITOR

public class HealthPointsDrawer : OdinValueDrawer<HealthPoints>
{
	protected override void DrawPropertyLayout(GUIContent label)
	{
		HealthPoints value = this.ValueEntry.SmartValue;

		var rect = EditorGUILayout.GetControlRect();

		// In Odin, labels are optional and can be null, so we have to account for that.
		if (label != null)
			rect = EditorGUI.PrefixLabel(rect, label);

		DrawEditor(ref value, rect);

		this.ValueEntry.SmartValue = value;
	}

	public static void DrawEditor(ref HealthPoints value, Rect rect) 
	{
		var prev = EditorGUIUtility.labelWidth;
		EditorGUIUtility.labelWidth = 15;
		float heartsWidth = 50;
		int newValue = Mathf.Clamp(value.hearts, 0, 999);
		value.hearts = EditorGUI.IntField(rect.AlignLeft(heartsWidth), "♥", newValue );
		value.fractions = EditorGUI.IntSlider(rect.AlignRight(rect.width - heartsWidth), "¾", value.fractions, 0, HealthPoints.PointsPerHeart);

		EditorGUIUtility.labelWidth = prev;
	}
}
#endif
