using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Dictionary/Star Data")]
public class StarDataDictionary : SerializedScriptableObject 
{
	[SerializeField, Tooltip("The path that this will check for all star datas when generating dictionary.")]
	string pathToAssets = "Assets/Data";

	[SerializeField, ReadOnly]
	Dictionary<string, StarData> starDatas = new Dictionary<string, StarData>();

	[Button]
	void GenerateDictionary()
	{
		#if UNITY_EDITOR

		// Check which star data are already indexed, so we can skip it.
		List<StarData> indexedAlready = new List<StarData>();
		indexedAlready.AddRange(starDatas.Values);

		var guids = AssetDatabase.FindAssets("t:StarData", new[] {"Assets/Data"});
		foreach (var guid in guids)
		{
			var assetPath = AssetDatabase.GUIDToAssetPath(guid);
			var asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(StarData)) as StarData;

			if (indexedAlready.Contains(asset)) 
				continue;

			// add the newly found star data to the dictionary
			// first generate a GUID (the asset database GUID may change and isn't safe to use in this context)
			Guid newGuid = Guid.NewGuid();
			starDatas.Add(newGuid.ToString(), asset);
		}
		#endif
	}
}