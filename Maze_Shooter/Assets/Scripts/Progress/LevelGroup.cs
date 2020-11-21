using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class LevelGroup : MonoBehaviour
{
	public int level = 1;

	public bool setOnAwake = true;

	void Awake() {
		if (setOnAwake) SetLevelOfChildren();
	}

	[Button]
	void SetLevelOfChildren() {
		foreach (LevelObject obj in GetComponentsInChildren<LevelObject>()) {
			obj.minLevel = level;
		}
	}
}
