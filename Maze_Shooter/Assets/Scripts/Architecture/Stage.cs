using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Ghost/Stage")]
public class Stage : ScriptableObject
{
    public string displayName;
    public Color stageColor = Color.white;
    public string sceneName;
    
    [Tooltip("Events that will take place immediately when stage is loaded")]
    public List<GameEvent> immediateEvents;

    public GameObject PlayerShip => GameMaster.Get().defaultPlayerShip;

    public void Load(float delay)
    {
        GameMaster.Get().LoadScene(sceneName, delay);
    }
}