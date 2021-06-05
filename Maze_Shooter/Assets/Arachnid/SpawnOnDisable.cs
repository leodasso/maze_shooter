using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnDisable : MonoBehaviour {

    public GameObject toSpawn;

	void OnDisable()
    {
        if (!toSpawn) return;
		if (GhostTools.SafeToInstantiate(gameObject))
        	Instantiate(toSpawn, transform.position, transform.rotation);
    }
}
