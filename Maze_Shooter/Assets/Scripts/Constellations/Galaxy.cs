using System;
using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class Galaxy : MonoBehaviour
{
    public FloatReference focusSpeed;
    public List<ConstellationNode> constellationNodes = new List<ConstellationNode>();
    [Tooltip("If this isn't null, it will be shown at the center of the portal")]
    public GameObject focusObject;
    [Tooltip("The parent object of the backdrop and constellations. Is moved around to bring certain" +
             " constellations into view.")]
    public GameObject galaxyMain;
    public ConstellationData constellationToFocus;

    public UnityEvent showConstellationAcquire;

    Vector3 _difference;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!focusObject) return;
        _difference = Vector3.Scale(focusObject.transform.localPosition, new Vector3(-1, -1, 1));
        galaxyMain.transform.localPosition =
            Vector3.Lerp(galaxyMain.transform.localPosition, _difference, Time.unscaledDeltaTime * focusSpeed.Value);

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
