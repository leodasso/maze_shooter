using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Ghost/Stage")]
public class Stage : ScriptableObject
{
    public string displayName;
    public Color stageColor = Color.white;
    public string sceneName;

	[Space]
	public World world;

	public GameObject hauntStarPrefab => world.hauntStarPrefab;
    
    [Tooltip("Events that will take place immediately when stage is loaded")]
    public List<GameEvent> immediateEvents;

    public GameObject PlayerShip => GameMaster.Get().defaultPlayerShip;

    public void Load(float delay)
    {
        GameMaster.Get().LoadScene(sceneName, delay);
    }

	public List<ConstellationData> GetAcquiredConstellations() 
	{
		List<ConstellationData> returnList = new List<ConstellationData>();
		foreach(var c in world.constellations) 
			if (c.HasBeenCollected()) returnList.Add(c);

		return returnList;
	}

	/// <summary>
	/// Returns a list of prefabs for the haunt stars for each acquired constellation
	/// </summary>
	public List<GameObject> GetHauntStarPrefabs() 
	{
		List<GameObject> prefabs = new List<GameObject>();
		foreach (var c in GetAcquiredConstellations()) 
			prefabs.Add(hauntStarPrefab);
		
		return prefabs;
	}
}