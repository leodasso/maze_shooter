using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Stage")]
public class Stage : ScriptableObject
{
    public string displayName;
    public Color stageColor = Color.white;
    public string sceneName;
    public bool useDefaultPlayerShip = true;
    
    [HideIf("useDefaultPlayerShip")]
    public GameObject customShip;
    
    [Tooltip("Events that will take place immediately when stage is loaded")]
    public List<GameEvent> immediateEvents;

    [Tooltip("If this stage was just completed, this event will be called as soon as the world map is loaded.")]
    public GameEvent onComplete_worldMap;
    const string _key_complete = "_complete";

    public GameObject PlayerShip
    {
        get
        {
            if (!useDefaultPlayerShip && customShip != null) 
                return customShip;

            return GameMaster.Get().defaultPlayerShip;
        }
    }

    public void Load(float delay)
    {
        GameMaster.Get().LoadScene(sceneName, delay);
    }

    public bool IsComplete()
    {
        string saveDir;
        if (GameMaster.Get().TryGetSaveFileDirectory(out saveDir))
        {
            return ES3.Load<bool>(name + _key_complete, saveDir, false);
        }
        
        Debug.LogError("Unable to load stage complete status", this);
        return false;
    }

    public void CompleteStage()
    {
        string saveDir;
        if (GameMaster.Get().TryGetSaveFileDirectory(out saveDir))
        {
            // Save this stage as complete in the current file's save key
            ES3.Save<bool>(name + _key_complete, true, saveDir);
        }
        else
        {
            Debug.LogError("Unable to save stage complete:true", this);
        }
    }
}