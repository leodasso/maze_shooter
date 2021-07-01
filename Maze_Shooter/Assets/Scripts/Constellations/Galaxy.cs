using System;
using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class Galaxy : MonoBehaviour
{
    public List<StarNode> starNodes = new List<StarNode>();
    public Star StarInstance;
    public UnityEvent showConstellationAcquire;
	public StarPath starPath;

	public StarNode focusNode;

    Vector3 _difference;

    // Update is called once per frame
    void Update()
    {
    }

    public void MergeConstellationToNode()
    {
        if (!StarInstance) return;
        StarInstance.transform.parent = transform;
		starPath.setStartPosition(StarInstance.transform);
		starPath.objectOnPath = StarInstance.transform;
		starPath.MoveAlongPath();
    }

    StarNode NodeForStar(StarData starData)
    {
        foreach (var node in starNodes)
        {
            if (node.myStar == starData)
                return node;
        }
        
        throw new Exception("The star " + starData.name + " was not found in the galaxy.");
    }

	public void FillNode() 
	{
		StarInstance.onAddToGalaxy.Invoke();
		// TODO
		// if (focusNode) focusNode.Fill();
	}

    [Button]
    void GetStarNodes()
    {
        starNodes.Clear();
        starNodes.AddRange(GetComponentsInChildren<StarNode>());
    }
}