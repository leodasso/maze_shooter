using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class GameMaster : ScriptableObject
{
    public GameObject defaultPlayerShip;
    public float defaultRespawnTime = 3;
    public Stage currentStage;

    static GameMaster _gameMaster;
    
    /// <summary>
    /// True when transitioning between scenes/stages
    /// </summary>
    public static bool transitioning;

    public static GameMaster Get()
    {
        if (_gameMaster) return _gameMaster;
        _gameMaster = Resources.Load<GameMaster>("game master");
        return _gameMaster;
    }

    public void LoadScene(string sceneName, float delay)
    {
        
        CoroutineHelper.NewCoroutine(DelayLoadScene(sceneName, delay));
    }

    public void SetTransitioning(bool isTransitioning)
    {
        transitioning = isTransitioning;
    }

    IEnumerator DelayLoadScene(string sceneName, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        SceneManager.LoadScene(sceneName);
    }
}
