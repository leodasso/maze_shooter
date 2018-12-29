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

    [Tooltip("Event that gets called when this stage has been completed while in the world map")]
    public GameEvent onComplete_worldMap;

    const string _key_complete = "complete";

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

    public bool TryGetStageKey(out string key)
    {
        key = "";
        string saveDir = "";
        if (GameMaster.Get().TryGetSaveFileDirectory(out saveDir))
        {
            key = saveDir + "_" + name + "_";
            return true;
        }

        return false;
    }

    public bool IsComplete()
    {
        string key = "";
        if (TryGetStageKey(out key))
        {
            return ES3.Load<bool>(key + _key_complete, false);
        }

        return false;
    }

    public void CompleteStage()
    {
        // TODO probably saving
        string key = "devbaby";
        if (TryGetStageKey(out key))
        {
            ES3.Save<bool>(key + _key_complete, true);
        }
    }
}