using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

[CreateAssetMenu]
public class GameMaster : ScriptableObject
{
    public GameObject defaultPlayerShip;
    public Stage currentStage;
    [ReadOnly]
    public Stage justCompletedStage;
    static GameMaster _gameMaster;
    public SaveDataAvatar currentAvatar;

    public static string saveFilesDirectory = "saveFiles/";
    public static int cashThisSession;
    
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

    public void VerifyMainSaveFile()
    {
        if (ES3.FileExists("main.es3"))
            Debug.Log("Main save data exists.");
        else
        {
            Debug.Log("Main save data does not exist.");
        }

        if (ES3.KeyExists("mostRecentStartup", "main.es3"))
            Debug.Log("Welcome back! Last play session was " + ES3.Load<System.DateTime>("mostRecentStartup", "main.es3"));

        ES3.Save<System.DateTime>("mostRecentStartup", System.DateTime.Now, "main.es3");

        if (ES3.DirectoryExists(saveFilesDirectory))
        {
            foreach (var filename in ES3.GetFiles(saveFilesDirectory))
            {
                Debug.Log("Found save file: " + filename);
            }
        }
        else Debug.Log("No save files exist.");
    }

    public void GotoWorldMap(float delay)
    {
        LoadScene("WorldMap B", delay);
    }

    public static bool AvatarIsUsedBySaveFile(SaveDataAvatar avatar)
    {
        return ES3.FileExists(saveFilesDirectory + avatar.name + ".es3");
    }

    public bool TryGetSaveFileDirectory(out string file)
    {
        file = "";
        if (!currentAvatar) return false;

        file = saveFilesDirectory + currentAvatar.name + ".es3";
        return true;
    }
}