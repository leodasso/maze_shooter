using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arachnid;
using UnityEngine.Events;

public class WorldMapPath : MonoBehaviour
{
    public FloatReference openPathDelay;
    
    [Tooltip("The unique key for this particular path")]
    public string uniqueSaveKey;

    [Tooltip("Optional - which stages need to be complete before this path can be shown?")]
    public List<Stage> prerequisites = new List<Stage>();

    public float animationDuration = 1;

    public UnityEvent pathOpenAnimation;

    public UnityEvent onPathOpen;
   
    bool PathIsOpen()
    {
        foreach (var stage in prerequisites)
        {
            if (!stage.IsComplete()) return false;
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

    void OpenPathWithAnimation()
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
        string key;
        if (TryGetSaveKey(out key))
            ES3.Save<bool>(key, true);
        
        else 
            Debug.LogWarning("Error saving " + name, gameObject);
    }

    /// <summary>
    /// Has this path been shown opening up or appearing?
    /// </summary>
    bool PathOpenShown()
    {
        string key;
        if (TryGetSaveKey(out key))
        {
            return ES3.Load<bool>(key, false);
        }
            
        Debug.LogWarning("Error getting save key for " + name, gameObject);
        return false;
    }

    /// <summary>
    /// Tries to get the save key for this path. If successful, returns true.
    /// </summary>
    /// <param name="key">The ES3 save key for this particular path</param>
    bool TryGetSaveKey(out string key)
    {
        key = "";
        string saveDir;
        if (GameMaster.Get().TryGetSaveFileDirectory(out saveDir))
        {
            key = saveDir + "_mapPathway_" + uniqueSaveKey;
            return true;
        }

        return false;
    }
}
