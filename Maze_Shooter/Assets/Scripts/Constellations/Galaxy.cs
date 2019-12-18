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
    [Tooltip("The speed at which the constellation instance is sucked in and merged to it's corresponding node")]
    public FloatReference mergeSpeed;
    public FloatReference focusSpeed;
    public List<ConstellationNode> constellationNodes = new List<ConstellationNode>();
    [Tooltip("If this isn't null, it will be shown at the center of the portal")]
    public GameObject focusObject;
    [Tooltip("The parent object of the backdrop and constellations. Is moved around to bring certain" +
             " constellations into view.")]
    public GameObject galaxyMain;
    public ConstellationData constellationToFocus;
    public Constellation constellationInstance;
    public UnityEvent showConstellationAcquire;

    Vector3 _difference;
    // If true, lerps the constellation instance to it's node
    bool _mergingConstellation;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

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

        if (_mergingConstellation && constellationInstance)
        {
            constellationInstance.transform.position = Vector3.Lerp(constellationInstance.transform.position,
                focusObject.transform.position, Time.unscaledDeltaTime * mergeSpeed.Value);
        }
    }

    public void ShowConstellation(ConstellationData constellation)
    {
        ConstellationNode node = NodeForConstellation(constellation);
        focusObject = node.gameObject;
    }

    public void ShowMyConstellation()
    {
        ShowConstellation(constellationToFocus);
    }

    public void MergeConstellationToNode()
    {
        if (!constellationInstance) return;
        constellationInstance.transform.parent = transform;
        constellationInstance.onAddToGalaxy.Invoke();
        _mergingConstellation = true;
    }

    ConstellationNode NodeForConstellation(ConstellationData constellation)
    {
        foreach (var node in constellationNodes)
        {
            if (node.linkedConstellation == constellation)
                return node;
        }
        
        throw new Exception("The constellation " + constellation.name + " was not found in the galaxy.");
    }

    [Button]
    void GetConstellationNodes()
    {
        constellationNodes.Clear();
        constellationNodes.AddRange(GetComponentsInChildren<ConstellationNode>());
    }
}
