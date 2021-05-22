﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ColorInverter : MonoBehaviour
{
	public Material regular;
	public Material inverted;
	public List<SpriteRenderer> sprites = new List<SpriteRenderer>();

	[ButtonGroup]
	void SetColorsInverted() {
		SetColors(inverted);
	}
	
	[ButtonGroup]
	void SetColorsRegular() {
		SetColors(regular);
	}

	void SetColors(Material material) {
		foreach (var s in sprites) s.material = material;
	}

}
