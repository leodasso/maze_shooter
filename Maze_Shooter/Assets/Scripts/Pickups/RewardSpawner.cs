using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arachnid;

public class RewardSpawner : MonoBehaviour
{
	[Tooltip("How many rewards to spawn"), SerializeField]
	IntReference rewardAmount;
	
	[SerializeField]
	float spawnDelay = .15f;

    public SpawnCollection rewards;
    public bool spawnOnDestroy;

	void Start()
	{

	}

    void OnDestroy()
    {
        if (spawnOnDestroy)
            Spawn();
    }

	public void SetSpawnOnDestroy(bool spawn) 
	{
		spawnOnDestroy = spawn;
	}

    public void Spawn()
    {
		if (!enabled) return;
		CoroutineHelper.NewCoroutine(SpawnSequence(transform.position, spawnDelay));
    }

	IEnumerator SpawnSequence(Vector3 position, float spawnDelay)
	{
		for (int i = 0; i < rewardAmount.Value; i++)
		{
 			Instantiate(rewards.GetRandom(), position, Quaternion.identity);
			yield return new WaitForSeconds(spawnDelay);
		}
	}
}
