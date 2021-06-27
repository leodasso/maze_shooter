using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using Arachnid;

public class StarNode : MonoBehaviour
{
	[SerializeField]
	StarDataDictionary starDatas;

	[SerializeField]
	UnityEvent onSlotFilled;

	[SerializeField]
	GuidGenerator guidGenerator;

	[ReadOnly, Tooltip("For nodes where the player has placed a star in them, it will appear here upon load.")]
	public StarData myStar;

	const string prefix = "starNodes.node_";

	string mySaveKey => prefix + guidGenerator.uniqueId;

	void Awake()
	{
		if (!guidGenerator) {
			Debug.LogError(name + " has no GUID generator! This will cause a critical failure with loading save data.", gameObject);
			enabled = false;
			return;
		}

		Load();
	}

	[Button]
	void Load()
	{
		string guidForStarData = GameMaster.LoadFromCurrentFileCache(mySaveKey, "", this);
		myStar = starDatas.GetStar(guidForStarData);
	}

	[Button]
	public void Fill(StarData star) 
	{
		GameMaster.SaveToCurrentFileCache(mySaveKey, starDatas.GetGuid(star), this);
		onSlotFilled.Invoke();
	}
}
