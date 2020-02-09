using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneySpawner : MonoBehaviour
{
    public SpawnCollection coins;
    public bool spawnOnDestroy;

    void OnDestroy()
    {
        if (spawnOnDestroy)
            Spawn();
    }

    void Spawn()
    {
        Instantiate(coins.GetRandom(), transform.position, Quaternion.identity);
    }
}
