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
	[LabelText("Save File"), Title("General")]
	public SaveDataAvatar currentAvatar;
	public GameEvent saveFileAccessedEvent;
	public IntReference hpPerHeart;
	public UnityEvent onBeginLoadSavedGame;
	public GameObject hauntConstellationPrefab;

	[Space, Title("Player")]
    public GameObject defaultPlayerShip;
	public Collection playerInstancesCollection;

	[Space, Title("Stages")]
	public List<Stage> allStages = new List<Stage>();
    public Stage currentStage;
    [ReadOnly]
    public Stage justCompletedStage;

	public SavedString savedStage;
    public SavedString savedCheckpoint;

	[ReadOnly]
    public GateLink gateLink;

	[AssetsOnly, Space, Title("Audio & Music")]
	public GameObject audioListenerPrefab;

	[AssetsOnly]
	public GameObject musicPlayerPrefab;

    static GameMaster _gameMaster;

    public static string saveFilesDirectory = "saveFiles/";

	static GameObject audioListenerInstance;

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

	public static int FractionsPerHeart => Get().hpPerHeart.Value;

    [Button, DisableInEditorMode]
    public void BeginGame()
    {
        onBeginLoadSavedGame.Invoke();
        // find the stage to load from list
        Stage stageToLoad = GetStage(savedStage.runtimeValue);

        // Load with a delay so there's time for the transition to fade in
        stageToLoad.Load(1);
    }

	public void QuitToDesktop()
	{
		// TODO time since last checkpoint reminder
		Application.Quit();
	}

	public static void AddAudioListener() 
	{
		if (audioListenerInstance) return;
		audioListenerInstance = Instantiate(Get().audioListenerPrefab);
		DontDestroyOnLoad(audioListenerInstance.gameObject);
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
		/*
        Get().savedStage.Save(Get().currentStage.name);
        Get().savedCheckpoint.Save(checkpointName);
		*/
		// TODO save with given checkpoint name
    }

    public static bool IsCurrentCheckpoint(string checkpointName)
    {
        return checkpointName == Get().savedCheckpoint.runtimeValue;
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

	/// <summary>
	/// Calls for ALL saved properties on the current file to get their value from the persistent save data
	/// </summary>
	public static void LoadData()
	{

	}

	/// <summary>
	/// Calls for ALL saved properties to write their value to the persistent save data for the current file
	/// </summary>
	public static void SaveData()
	{

	}

	public static void OnSaveFileAccessed()
	{
		// TODO maybe logging or analytics?

		if (!Get().saveFileAccessedEvent) return;
		Get().saveFileAccessedEvent.Raise();
	}
    
    public void VerifyMainSaveFile()
    {
		OnSaveFileAccessed();

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
		OnSaveFileAccessed();

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

		OnSaveFileAccessed();
        
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
		Debug.Log(requester.name + " is saving " + saveKey + " at " + System.DateTime.Now);

        string saveDir;
        if (Get().TryGetSaveFileDirectory(out saveDir)) 
            ES3.Save<T>(saveKey, value, saveDir);
        
        else 
            Debug.LogError("Error saving " + saveKey + " value from " + requester.name, requester);

		OnSaveFileAccessed();
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
		OnSaveFileAccessed();

        string saveDir;
        if (Get().TryGetSaveFileDirectory(out saveDir))
            return ES3.Load<T>(saveKey, saveDir, defaultValue);
            
        throw new FileLoadException();
    }

    public static bool DoesKeyExist(string saveKey)
    {
		OnSaveFileAccessed();
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