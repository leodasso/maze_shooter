using System.Collections;
using System.Collections.Generic;
using System.IO;
using Arachnid;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Ghost/Game Master")]
public class GameMaster : ScriptableObject
{
    public GameObject defaultPlayerShip;
    public Stage currentStage;
    [ReadOnly]
    public Stage justCompletedStage;
    static GameMaster _gameMaster;
    public SaveDataAvatar currentAvatar;
    public GateLink gateLink;
    public Collection playerInstancesCollection;

    public SavedString savedStage;
    public SavedString savedCheckpoint;
    
    public List<Stage> allStages = new List<Stage>();

    public UnityEvent onBeginLoadSavedGame;

    public static string saveFilesDirectory = "saveFiles/";

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

    [Button, DisableInEditorMode]
    public void BeginGame()
    {
        onBeginLoadSavedGame.Invoke();
        // find the stage to load from list
        Stage stageToLoad = GetStage(savedStage.GetValue());
        // Load with a delay so there's time for the transition to fade in
        stageToLoad.Load(1);
    }

    public static GameObject GetPlayerInstance()
    {
        var firstPlayerObject = Get().playerInstancesCollection.GetFirstElement();
        if (firstPlayerObject == null) return null;
        return firstPlayerObject.gameObject;
    }

    Stage GetStage(string stageName)
    {
        foreach (Stage stage in allStages)
        {
            if (stage.name == stageName) return stage;
        }
        
        Debug.LogError("No stage found matching name " + stageName);
        return null;
    }

    public static void SetCheckpoint(string checkpointName)
    {
        Get().savedStage.Save(Get().currentStage.name);
        Get().savedCheckpoint.Save(checkpointName);
    }

    public static bool IsCurrentCheckpoint(string checkpointName)
    {
        return checkpointName == Get().savedCheckpoint.GetValue();
    }

    public static void SetGateLink(GateLink newGateLink)
    {
        Get().gateLink = newGateLink;
    }

    public static void CompleteGateLink()
    {
        Get().gateLink = null;
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

    #region Load-Save
    
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
    
    public static bool AvatarIsUsedBySaveFile(SaveDataAvatar avatar)
    {
        return ES3.FileExists(saveFilesDirectory + avatar.name + ".es3");
    }
    
    public static void DeleteSaveData(SaveDataAvatar avatar)
    {
        if (!avatar) return;
        Debug.Log("Deleting save data for " + avatar.name);

        if (!AvatarIsUsedBySaveFile(avatar))
        {
            Debug.Log("Avatar doesn't have any save data.");
            return;
        }
        
        ES3.DeleteFile(saveFilesDirectory + avatar.name + ".es3");
    }
    
    
    /// <summary>
    /// Saves the given value to the player's current save file.
    /// </summary>
    /// <param name="saveKey">The key to store the value under.</param>
    /// <param name="value">The value to save.</param>
    /// <param name="requester">The object that is requesting for a value to be saved.</param>
    public static void SaveToCurrentFile<T>(string saveKey, T value, Object requester)
    {
        string saveDir;
        if (Get().TryGetSaveFileDirectory(out saveDir))
            ES3.Save<T>(saveKey, value, saveDir);
        
        else 
            Debug.LogError("Error saving " + saveKey + " value from " + requester.name, requester);
    }
    

    /// <summary>
    /// Loads the value for the given key from the player's current save file.
    /// </summary>
    /// <param name="saveKey">The key to load the value from</param>
    /// <param name="defaultValue">The default value. If no value has been saved to this key yet,
    /// this will be returned.</param>
    /// <param name="requester">The object requesting for the value.</param>
    public static T LoadFromCurrentFile<T>(string saveKey, T defaultValue, Object requester)
    {
        string saveDir;
        if (Get().TryGetSaveFileDirectory(out saveDir))
            return ES3.Load<T>(saveKey, saveDir, defaultValue);
            
        throw new FileLoadException();
    }

    public static bool DoesKeyExist(string saveKey)
    {
        string saveDir;
        if (Get().TryGetSaveFileDirectory(out saveDir))
            return ES3.KeyExists(saveKey, saveDir);
        
        throw new FileLoadException();
    }

    /// <summary>
    /// Returns true if the directory was found. 
    /// </summary>
    /// <param name="file">Contains the save file directory for the current avatar.</param>
    public bool TryGetSaveFileDirectory(out string file)
    {
        file = "";
        if (!currentAvatar) return false;

        file = saveFilesDirectory + currentAvatar.name + ".es3";
        return true;
    }
    #endregion
}