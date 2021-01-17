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
public struct Hearts
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

	public void SetTotalPoints(int totalPoints) 
	{
		Recalculate(totalPoints);
	}

	public static Hearts Clamp(Hearts input, Hearts min, Hearts max) 
	{
		int total = input.TotalPoints;
		int clamped = Mathf.Clamp(total, min.TotalPoints, max.TotalPoints);
		input.SetTotalPoints(clamped);
		return input;
	}

	public static Hearts operator+(Hearts left, Hearts right) 
	{
		Hearts newHP = new Hearts();
		newHP.Recalculate(left.TotalPoints + right.TotalPoints);
		return newHP;
	}

	public static Hearts operator-(Hearts left, Hearts right) 
	{
		Hearts newHP = new Hearts();
		int newPoints = left.TotalPoints - right.TotalPoints;
		if (newPoints < 0) newPoints = 0;
		newHP.Recalculate(newPoints);
		return newHP;
	}

	public static Hearts operator+(Hearts left, int right) 
	{
		Hearts newHP = new Hearts();
		newHP.Recalculate(left.TotalPoints + right);
		return newHP;
	}

	public static Hearts operator-(Hearts left, int right) 
	{
		Hearts newHP = new Hearts();
		int newPoints = left.TotalPoints - right;
		if (newPoints < 0) newPoints = 0;
		newHP.Recalculate(newPoints);
		return newHP;
	}

	public static bool operator==(Hearts left, Hearts right) 
	{
		return left.TotalPoints == right.TotalPoints;
	}

	public static bool operator!=(Hearts left, Hearts right) 
	{
		return left.TotalPoints != right.TotalPoints;
	}

	public static implicit operator Hearts(int qty) {
		Hearts newHp = new Hearts();
		newHp.hearts = qty;
		newHp.fractions = 0;
		return newHp;
	}
}

#if UNITY_EDITOR

public class HealthPointsDrawer : OdinValueDrawer<Hearts>
{
	protected override void DrawPropertyLayout(GUIContent label)
	{
		Hearts value = this.ValueEntry.SmartValue;

		var rect = EditorGUILayout.GetControlRect();

		// In Odin, labels are optional and can be null, so we have to account for that.
		if (label != null)
			rect = EditorGUI.PrefixLabel(rect, label);

		DrawEditor(ref value, rect);

		this.ValueEntry.SmartValue = value;
	}

	public static void DrawEditor(ref Hearts value, Rect rect) 
	{
		var prev = EditorGUIUtility.labelWidth;
		EditorGUIUtility.labelWidth = 15;
		float heartsWidth = 50;
		int newValue = Mathf.Clamp(value.hearts, 0, 999);
		value.hearts = EditorGUI.IntField(rect.AlignLeft(heartsWidth), "♥", newValue );
		value.fractions = EditorGUI.IntSlider(rect.AlignRight(rect.width - heartsWidth), "¾", value.fractions, 0, Hearts.PointsPerHeart);

		EditorGUIUtility.labelWidth = prev;
	}
}
#endif
