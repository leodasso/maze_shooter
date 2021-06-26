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
    public List<StarNode> constellationNodes = new List<StarNode>();
    [Tooltip("If this isn't null, it will be shown at the center of the portal")]
    public GameObject focusObject;
    [Tooltip("The parent object of the backdrop and constellations. Is moved around to bring certain" +
             " constellations into view.")]
    public GameObject galaxyMain;
    public StarData constellationToFocus;
    public Star constellationInstance;
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
        focusNode = NodeForConstellation(starData);
		starPath.starSlot = focusNode.transform;
        focusObject = focusNode.gameObject;
    }

    public void ShowMyConstellation()
    {
        ShowConstellation(constellationToFocus);
    }

    public void MergeConstellationToNode()
    {
        if (!constellationInstance) return;
        constellationInstance.transform.parent = transform;
		starPath.setStartPosition(constellationInstance.transform);
		starPath.objectOnPath = constellationInstance.transform;
		starPath.MoveAlongPath();
    }

    StarNode NodeForConstellation(StarData constellation)
    {
        foreach (var node in constellationNodes)
        {
            if (node.linkedStar == constellation)
                return node;
        }
        
        throw new Exception("The constellation " + constellation.name + " was not found in the galaxy.");
    }

	public void FillNode() 
	{
		constellationInstance.onAddToGalaxy.Invoke();
		if (focusNode) focusNode.Fill();
	}

    [Button]
    void GetConstellationNodes()
    {
        constellationNodes.Clear();
        constellationNodes.AddRange(GetComponentsInChildren<StarNode>());
    }
}