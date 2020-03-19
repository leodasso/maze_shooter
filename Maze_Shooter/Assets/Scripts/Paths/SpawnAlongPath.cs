using System.Collections;
using System.Collections.Generic;
using Arachnid;
using Cinemachine;
using UnityEngine;
using Sirenix.OdinInspector;

public class SpawnAlongPath : MonoBehaviour
{
    public CinemachinePathBase path;

    public float spacing = 1;
    [Tooltip("The radius of the circle that objects will spawn randomly inside along path. " +
             "Set to 0 for perfect path placement."), MinValue(0)]
    public float randomRadius = 1;

    [Space, MinValue(.01f)]
    public float minScale = 1;
    [MinValue(.01f)]
    public float maxScale = 1;
    
    [Tooltip("One or more prefabs to be spawned along the path. If more than one, they'll be randomly selected.")]
    public List<GameObject> prefabsToSpawn = new List<GameObject>();

    [ReadOnly]
    public List<GameObject> instances = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {

    }

    [Button]
    void Spawn()
    {
        #if UNITY_EDITOR
        foreach (var instance in instances)
        {
            SafeDestroy(instance);
        }
        
        instances.Clear();
        
        if (!path) return;
        if (prefabsToSpawn.Count < 1) return;

        float progressAlongPath = 0;
        int iterations = 0;
        while (iterations < 9999)
        {
            // spawn the instance
            Vector3 spawnPos = path.EvaluatePositionAtUnit(progressAlongPath, CinemachinePathBase.PositionUnits.Distance);
            GameObject prefabToSpawn = Math.RandomElementOfList(prefabsToSpawn);

            var newInstance = UnityEditor.PrefabUtility.InstantiatePrefab(prefabToSpawn, transform) as GameObject;
            instances.Add(newInstance);

            Vector3 randomCircle = Random.insideUnitSphere * randomRadius;
            newInstance.transform.position = spawnPos + new Vector3(randomCircle.x, 0, randomCircle.z);

            float scaleMultiplier = Random.Range(minScale, maxScale);
            newInstance.transform.localScale *= scaleMultiplier;

            progressAlongPath += spacing;
            if (progressAlongPath >= path.PathLength) 
                break;
            
            iterations++;
        }
        #endif
    }

    void SafeDestroy(GameObject go)
    {
        if (Application.isPlaying)
            Destroy(go);
        else
            DestroyImmediate(go);
    }
}
