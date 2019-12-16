using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Constellation : MonoBehaviour
{
    public ConstellationData myConstellation;
    public GameObject galaxyPrefab;
    public Transform galaxySpawnPoint;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OpenGalaxy()
    {
        Galaxy newGalaxy = Instantiate(galaxyPrefab, galaxySpawnPoint.position, quaternion.identity).GetComponent<Galaxy>();
        newGalaxy.constellationToFocus = myConstellation;
        newGalaxy.showConstellationAcquire.Invoke();
    }
}
