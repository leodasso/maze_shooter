﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[InfoBox("Has functions to activate children in a sequence with a delay between them.")]
public class ActivateChildren : MonoBehaviour
{
    public float delayBetweenActivations = .15f;

    [ToggleLeft]
    public bool activateOnEnable = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable()
    {
        if (activateOnEnable) ActivateMyChildren();
    }

    [Button]
    public void ActivateMyChildren()
    {
        StartCoroutine(DoActivation());
    }

    IEnumerator DoActivation()
    {
        List<GameObject> childrenToBeActivated = new List<GameObject>();
        foreach (Transform t in transform)
        {
            // Only add children that aren't already active
            if (t.gameObject.activeSelf) continue;
            
            childrenToBeActivated.Add(t.gameObject);
        }

        foreach (GameObject go in childrenToBeActivated)
        {
            go.SetActive(true);
            yield return new WaitForSeconds(delayBetweenActivations);
        }
    }
}
