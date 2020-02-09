using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneySpawner : MonoBehaviour
{
    public GameObject coinPrefab;
    public bool spawnOnDestroy;

    void OnDestroy()
    {
        if (spawnOnDestroy)
            Spawn();
    }

    void Spawn()
    {
        Instantiate(coinPrefab, transform.position, Quaternion.identity);
    }
}
