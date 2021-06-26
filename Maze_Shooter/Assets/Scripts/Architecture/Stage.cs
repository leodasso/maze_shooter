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
    
    [Tooltip("Events that will take place immediately when stage is loaded")]
    public List<GameEvent> immediateEvents;

    public GameObject PlayerShip => GameMaster.Get().defaultPlayerShip;

    public void Load(float delay)
    {
        GameMaster.Get().LoadScene(sceneName, delay);
    }

	public List<StarData> GetAcquiredStars() 
	{
		List<StarData> returnList = new List<StarData>();
		foreach(var c in world.stars) 
			if (c.HasBeenCollected()) returnList.Add(c);

		return returnList;
	}
}