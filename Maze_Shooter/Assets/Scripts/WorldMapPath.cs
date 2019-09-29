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

    const string saveKeyPrefix = "mapPathway_";
   
    bool PathIsOpen()
    {
        foreach (var stage in prerequisites)
        {
            bool isComplete = stage.IsComplete();
            Debug.Log("Stage " + stage + " is seen as complete: " + isComplete, gameObject);
            if (!isComplete) return false;
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
        string saveDir;
        if (GameMaster.Get().TryGetSaveFileDirectory(out saveDir))
            ES3.Save<bool>(saveKeyPrefix + uniqueSaveKey, true, saveDir);
        
        else 
            Debug.LogWarning("Error saving " + name, gameObject);
    }

    /// <summary>
    /// Has this path been shown opening up or appearing?
    /// </summary>
    bool PathOpenShown()
    {
        string saveDir;
        if (GameMaster.Get().TryGetSaveFileDirectory(out saveDir))
        {
            return ES3.Load<bool>(saveKeyPrefix + uniqueSaveKey, saveDir, false);
        }
            
        Debug.LogError("Unable to find save file directory. " + name, gameObject);
        return false;
    }
}
