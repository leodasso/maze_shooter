using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Constellation : MonoBehaviour
{
	[SerializeField]
	List<StarNode> starNodes = new List<StarNode>();

	[SerializeField]
	SpriteRenderer illustrationSprite;

    void Start()
    {
        
    }

	void CheckStarNodes()
	{
		foreach (var node in starNodes)
			if (!node.isActive) return;

		ActivateMe();
	}

	void ActivateMe()
	{
		// TODO
	}

	[Button]
	void GetChildStarNodes()
	{
		starNodes.Clear();
		starNodes.AddRange(GetComponentsInChildren<StarNode>());
	}
}
