﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arachnid;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class WorldMapPath : MonoBehaviour
{
    [TabGroup("main")]
    public FloatReference openPathDelay;
    
    [TabGroup("main")]
    [Tooltip("The unique key for this particular path")]
    [ValidateInput("ValidateSaveKey", "Save key must be unique!")]
    public string uniqueSaveKey;

    [TabGroup("main")]
    [Tooltip("Optional - which stages need to be complete before this path can be shown?")]
    public List<Stage> prerequisites = new List<Stage>();

    [TabGroup("main")]
    public float animationDuration = 1;
    
    [TabGroup("events")]
    public UnityEvent pathOpenAnimation;
    
    [TabGroup("events")]
    public UnityEvent onPathOpen;

    const string saveKeyPrefix = "mapPathway_";
   
    bool PathIsOpen()
    {
        /*
        foreach (var stage in prerequisites)
        {
            //bool isComplete = stage.IsComplete();
            //Debug.Log("Stage " + stage + " is seen as complete: " + isComplete, gameObject);
            if (!isComplete) return false;
        }
        */
        return true;
    }

    bool ValidateSaveKey(string inputKey)
    {
        foreach (var path in FindObjectsOfType<WorldMapPath>())
        {
            if (path == this) continue;
            if (path.uniqueSaveKey == inputKey) return false;
        }

        return true;
    }
    
    // Start is called before the first frame update
    void Start()
    {        
        if (PathIsOpen())
        {
            if (!PathOpenShown())
                OpenPathWithAnimation();
            
            else 
                OpenPath();
        }
    }

    public void OpenPathWithAnimation()
    {
        StartCoroutine(DoPathOpenAnimation());
    }

    IEnumerator DoPathOpenAnimation()
    {
        yield return new WaitForSecondsRealtime(openPathDelay.Value);
        pathOpenAnimation.Invoke();
        yield return new WaitForSecondsRealtime(animationDuration);
        SaveAnimationShown();
        OpenPath();
    }

    /// <summary>
    /// Immediately opens the path without any animation
    /// </summary>
    void OpenPath()
    {
        onPathOpen.Invoke();
    }

    /// <summary>
    /// Saves that this path opening has been shown
    /// </summary>
    void SaveAnimationShown()
    {
        GameMaster.SaveToCurrentFile(saveKeyPrefix + uniqueSaveKey, true, this);
    }

    /// <summary>
    /// Has this path been shown opening up or appearing?
    /// </summary>
    bool PathOpenShown()
    {
        return GameMaster.LoadFromCurrentFile(saveKeyPrefix + uniqueSaveKey, false, this);
    }
}
