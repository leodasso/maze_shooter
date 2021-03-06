﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ghost/World")]
public class World : ScriptableObject
{
	public string displayName;

	[Tooltip("Constellations for this World")]
	public List<ConstellationData> constellations = new List<ConstellationData>();
	
	public GameObject hauntStarPrefab;
}
