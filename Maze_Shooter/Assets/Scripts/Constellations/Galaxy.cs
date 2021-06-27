using System;
using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class Galaxy : MonoBehaviour
{
    [Tooltip("Where to show the focused node (in relation to this object's pivot point)")]
    public Vector3 focusOffset;
    public FloatReference focusSpeed;
    public List<StarNode> starNodes = new List<StarNode>();
    [Tooltip("If this isn't null, it will be shown at the center of the portal")]
    public GameObject focusObject;
    [Tooltip("The parent object of the backdrop and constellations. Is moved around to bring certain" +
             " constellations into view.")]
    public GameObject galaxyMain;
    public StarData starToFocus;
    public Star StarInstance;
    public UnityEvent showConstellationAcquire;
	public StarPath starPath;

	public StarNode focusNode;

    Vector3 _difference;

    // Update is called once per frame
    void Update()
    {
        if (focusObject)
        {
            _difference = Vector3.Scale(focusObject.transform.localPosition, new Vector3(-1, -1, 1)) + focusOffset;
            galaxyMain.transform.localPosition =
                Vector3.Lerp(galaxyMain.transform.localPosition, _difference,
                    Time.unscaledDeltaTime * focusSpeed.Value);
        }
    }

    public void ShowConstellation(StarData starData)
    {
        focusNode = NodeForStar(starData);
		starPath.starSlot = focusNode.transform;
        focusObject = focusNode.gameObject;
    }

    public void ShowMyConstellation()
    {
        ShowConstellation(starToFocus);
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