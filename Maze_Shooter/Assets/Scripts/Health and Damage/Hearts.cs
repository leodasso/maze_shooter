using UnityEngine;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
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

	public static bool operator <(Hearts left, Hearts right) 
	{
		return left.TotalPoints < right.TotalPoints;
	}

	public static bool operator >(Hearts left, Hearts right) 
	{
		return left.TotalPoints > right.TotalPoints;
	}

	public static bool operator <=(Hearts left, Hearts right) 
	{
		return left.TotalPoints <= right.TotalPoints;
	}

	public static bool operator >=(Hearts left, Hearts right) 
	{
		return left.TotalPoints >= right.TotalPoints;
	}

	public static implicit operator Hearts(int qty) {
		Hearts newHp = new Hearts();
		newHp.hearts = qty;
		newHp.fractions = 0;
		return newHp;
	}

	public override string ToString()
	{
		return  hearts +"♥ " + fractions + "/" + PointsPerHeart;
	}
}

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(Hearts))]
public class HeartsDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		// Using BeginProperty / EndProperty on the parent property means that
		// prefab override logic works on the entire property.
		EditorGUI.BeginProperty(position, label, property);

		// Draw label
		if (label != null) 
			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

		// Don't make child fields be indented
		var indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		// Calculate rects
		var heartsLabelRect = new Rect(position.x, position.y, 20, position.height);
		var heartsRect = new Rect(position.x + 20, position.y, 30, position.height);
		var fractionsLabelRect = new Rect(position.x + 55, position.y, 20, position.height);
		var fractionsRect = new Rect(position.x + 75, position.y, position.width -75, position.height);
		
		
		EditorGUI.LabelField(heartsLabelRect, "♥");
		EditorGUI.PropertyField(heartsRect, property.FindPropertyRelative("hearts"), GUIContent.none);
		EditorGUI.LabelField(fractionsLabelRect, "¾");
		EditorGUI.IntSlider(fractionsRect,  property.FindPropertyRelative("fractions"), 0, Hearts.PointsPerHeart, GUIContent.none);

		// Set indent back to what it was
		EditorGUI.indentLevel = indent;
		EditorGUI.EndProperty();
	}
}
#endif
