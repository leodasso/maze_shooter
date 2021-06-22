using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGui : MonoBehaviour
{
	bool guiEnabled;
	public int saveableGuiHeight = 24;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.F4))
			guiEnabled = !guiEnabled;
	}

    void OnGUI()
	{
		if (!guiEnabled) return;

		GUI.Box(new Rect(10, 10, 300, GameMaster.allSaveables.Count * saveableGuiHeight + 20), "Save File Debug - " + GameMaster.allSaveables.Count + " saveables found.");

		List<ISaveable> saveables = new List<ISaveable>();
		saveables.AddRange(GameMaster.allSaveables);
		for (int i = 0; i < saveables.Count; i++)
		{
			ISaveable s = saveables[i];

			GUI.Box(new Rect(12, 30 + i * saveableGuiHeight, 280, saveableGuiHeight), s.Info());
		}
	}
}
