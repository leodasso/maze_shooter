using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using Arachnid;

public class StarNode : MonoBehaviour
{
	[ToggleLeft]
	public bool isActive;

	[SerializeField]
	StarDataDictionary starDatas;

	[SerializeField, Tooltip("Sends events 'loadEmpty', 'loadFull', 'onFill'")]
	PlayMakerFSM playMaker;

	[SerializeField]
	GuidGenerator guidGenerator;

	[ReadOnly, Tooltip("For nodes where the player has placed a star in them, it will appear here upon load.")]
	public StarData myStar;

	const string prefix = "starNode_";

	string mySaveKey => prefix + guidGenerator.uniqueId;

	void Awake()
	{
		if (!guidGenerator) {
			Debug.LogError(name + " has no GUID generator! This will cause a critical failure with loading save data.", gameObject);
			enabled = false;
			return;
		}

		Load();

		isActive = myStar != null;
		playMaker.SendEvent( isActive ? "loadFull" : "loadEmpty");
	}

	[Button]
	void Load()
	{
		string guidForStarData = GameMaster.LoadFromCurrentFileCache(mySaveKey, "", this);
		if (guidForStarData.Length > 0)
			myStar = starDatas.GetStar(guidForStarData);
	}

	public void AskToBeFilled()
	{
		
	}

	[Button]
	public void Fill(StarData star) 
	{
		GameMaster.SaveToCurrentFileCache(mySaveKey, starDatas.GetGuid(star), this);
		isActive = true;
		playMaker.SendEvent("onFill");
	}
}
