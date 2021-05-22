using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class SaveFileMenu : MonoBehaviour
{

	[SerializeField, ToggleLeft]
	bool spawnFileGuis;

	[SerializeField, ShowIf("spawnFileGuis")]
	Transform saveFilesParent;

	[SerializeField, ShowIf("spawnFileGuis")]
	GameObject saveFileGuiPrefab;

	public UnityEvent onFileSelected;

    // Start is called before the first frame update
    void Start()
    {
		if (spawnFileGuis)
        	RefreshSaveFiles();
    }

	public void RefreshSaveFiles() 
	{
		foreach (var filename in ES3.GetFiles(GameMaster.saveFilesDirectory))
			SpawnFile(filename);
	}

	void SpawnFile(string filename)
	{
		int index = filename.LastIndexOf(".es3", StringComparison.Ordinal);
		string avatarName = filename.Substring(0, index);

		GameObject newFile = Instantiate(saveFileGuiPrefab, saveFilesParent);
		NewFileGui fileGui = newFile.GetComponent<NewFileGui>();
		fileGui.SetAvatar(Resources.Load<SaveDataAvatar>("avatars/" + avatarName));
	}

	public void SelectFile(SaveDataAvatar avatar) 
	{
		onFileSelected.Invoke();
	}
}
