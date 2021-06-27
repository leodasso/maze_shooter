using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using Math = Arachnid.Math;
using Random = UnityEngine.Random;

public class GrowthPatch : MonoBehaviour
{
    public int maxQty = 25;
    public AnimationCurve spawnArea;
    public List<GameObject> prefabs = new List<GameObject>();
    public bool saveValue;
    [ShowIf("saveValue")] 
    public string uniqueId;
    int qty;
    List<GameObject> _instances = new List<GameObject>();
    const string saveDataPrefix = "growthPatch_";
    
    // Start is called before the first frame update
    void Start()
    {
        SpawnMultiple(GetSavedQty());
    }

    int GetSavedQty()
    {
        return GameMaster.LoadFromCurrentFileCache(saveDataPrefix + uniqueId, maxQty, this);
    }

    public void Save()
    {
        int currentInstances = GetInstanceCount();
        Debug.Log("Saved qty: " +  currentInstances);
        GameMaster.SaveToCurrentFileCache(saveDataPrefix + uniqueId, currentInstances, this);
    }

    int GetInstanceCount()
    {
        int count = 0;
        foreach (var instance in _instances)
        {
            if (!instance) continue;
            if (!instance.activeInHierarchy) continue;
            count++;
        }

        return count;
    }

    [Button]
    void GenerateGuid()
    {
        Guid guid = Guid.NewGuid();
        uniqueId = guid.ToString();
    }

    [ButtonGroup("spawn")]
    void SpawnAll()
    {
        if (!Application.isPlaying)
        {
            Debug.Log("Enter play mode to test");
            return;
        }
        SpawnMultiple(maxQty);
    }

    void SpawnMultiple(int qty)
    {
        Debug.Log("Spawning " + qty);
        for (int i = 0; i < 5000; i++)
        {
            if (_instances.Count >= qty) return;
            SpawnOne();
        }
    }

    [ButtonGroup("spawn")]
    void SpawnOne()
    {
        float radius = Random.Range(0, MaxRadius());
        float diceRoll = Random.value;
        if (diceRoll > spawnArea.Evaluate(radius)) return;

        Vector3 pos = Random.onUnitSphere * radius;
        Vector3 flatPos = new Vector3(pos.x, 0, pos.z);
        var newInstance = Instantiate(Math.RandomElementOfList(prefabs), flatPos + transform.position, quaternion.identity, transform);
        _instances.Add(newInstance);
    }

    float MaxRadius()
    {
        if (spawnArea == null) return 1;
        if (spawnArea.keys.Length < 1) return 1;

        var lastKey = spawnArea.keys[spawnArea.keys.Length - 1];
        return lastKey.time;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
