using System.Collections;
using System.Collections.Generic;
using Arachnid;
using Paths;
using UnityEngine;

public class WorldMap : MonoBehaviour
{
	public Collection pathNodes;
	[Tooltip("If there's no previous stage defined, the player will load here")]
	public PathNode defaultPathNode;
	public PathFollower player;

	// Use this for initialization
	IEnumerator Start () 
	{
		yield return new WaitForSecondsRealtime(.1f);
		SelectCurrentNode();
	}

	void SelectCurrentNode()
	{
		if (GameMaster.Get().currentStage == null)
		{
			PlacePlayer(defaultPathNode);
			return;
		}

		foreach (var n in pathNodes.GetElementsOfType<PathNode>())
		{
			if (!n.linkedCrystal) continue;
			if (n.linkedCrystal.stage == GameMaster.Get().currentStage)
			{
				PlacePlayer(n);
				return;
			}
		}
	}

	void PlacePlayer(PathNode pathNode)
	{
		player.SetInitialNode(pathNode);
	}
}
