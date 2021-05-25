using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[ExecuteInEditMode]
public class ColorInverterGroup : MonoBehaviour
{
	public enum Style {
		Normal,
		Inverted
	}

	[EnumToggleButtons, OnValueChanged("Recalculate")]
	public Style style = Style.Normal;

	List<ColorInverter> children = new List<ColorInverter>();

	void Update()
	{
		if (Application.isPlaying) return;

		if (transform.childCount != children.Count)
			Recalculate();
	}

	void Recalculate() 
	{
		#if UNITY_EDITOR
		children.Clear();
		children = new List<ColorInverter>();

		children.AddRange(GetComponentsInChildren<ColorInverter>());

		for (int i = 0; i < children.Count; i++)
		{
			if (style == Style.Normal)
				children[i].SetColorsRegular();

			else 
				children[i].SetColorsInverted();
		}

		#endif
	}
}
