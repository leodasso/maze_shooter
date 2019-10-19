using System.Collections;
using System.Collections.Generic;
using Arachnid;
using Paths;
using UnityEngine;
using Sirenix.OdinInspector;

public class WorldMap : MonoBehaviour
{
	public Collection pathNodes;
	[Tooltip("If there's no previous stage defined, the player will load here")]
	public PathNode defaultPathNode;
	public PathFollower player;
	
	public List<PathRenderer> paths = new List<PathRenderer>();

	[Button]
	void GetPaths()
	{
		paths.Clear();
		paths.AddRange(GetComponentsInChildren<PathRenderer>());
	}

	// Use this for initialization
	IEnumerator Start () 
	{
		// place the player at the correct node
		yield return new WaitForSecondsRealtime(.1f);
		SelectCurrentNode();
 
		if (GameMaster.Get().justCompletedStage)
		{
			GameMaster.Get().justCompletedStage.onComplete_worldMap?.Raise();
			GameMaster.Get().justCompletedStage = null;
		}
	}

	void SelectCurrentNode()
	{
		// Place the player at the default starting node
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
