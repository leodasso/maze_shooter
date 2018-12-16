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

    [FoldoutGroup("data")]
    public bool stageComplete;

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

    public void CompleteStage()
    {
        // TODO probably saving
        stageComplete = true;
    }
}