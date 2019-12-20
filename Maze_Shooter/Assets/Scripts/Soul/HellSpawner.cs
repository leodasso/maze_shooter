using System.Collections.Generic;
using UnityEngine;
using Arachnid;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using Random = UnityEngine.Random;

[TypeInfoBox("When the player is in darkness, this object follows and spawns evil eyes " +
             "to give feedback that something bad is going to happen soon.")]
public class HellSpawner : MonoBehaviour
{
    public List<GameObject> evilEyesPrefabs = new List<GameObject>();
    public Collection collectionToFollow;
    [Space]
    [Tooltip("Radius around the player that evil eyes can be spawned.")]
    public float spawnRadius = 10;
    [Tooltip("The min scale of a spawned evil eye.")]
    public float minSpawnScale = .7f;
    [Tooltip("The max scale of a spawned evil eye.")]
    public float maxSpawnScale = 1.2f;
    [Space]
    public float maxSpawnFrequency;
    public float minSpawnFrequency;
    public FloatReference spawnFrequencyIncreaseSpeed;
    [ShowInInspector, ReadOnly]
    float _spawnFrequency;
    public FloatValue darknessLevel;

    float _spawnDelay = 1;
    
    // Evil eye instances that have been spawned in
    List<GameObject> _instances = new List<GameObject>();

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, .7f, 0, .5f);
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateSpawnFrequency();
        FollowPlayer();

        if (InDarkness())
        {
            _spawnDelay -= Time.deltaTime;
            if (_spawnDelay <= 0)
                Spawn();
        }

        // When the player returns to fully lit areas,
        // reset the spawn delay so the eye's don't immediately start appearing when they enter darkness.
        if (InFullLight())
        {
            _spawnDelay = 1;
            if (_instances.Count > 0)
            {
                foreach (var instance in _instances)
                {
                    Destroy(instance);
                }
            }
        }
    }

    void Spawn()
    {
        // Failsafe so we never spawn evil eyes in the light
        if (!InDarkness()) return;

        // Pick the prefab and a random location, then spawn an instance
        var toSpawn = Math.RandomElementOfList(evilEyesPrefabs);
        Vector3 pos = Random.onUnitSphere * spawnRadius;
        var newInstance = Instantiate(toSpawn, pos + transform.position, quaternion.identity);
        newInstance.transform.localScale *= Random.Range(minSpawnScale, maxSpawnScale);
        _instances.Add(newInstance);
        
        // Setup for the next spawn
        _spawnDelay = 1 / _spawnFrequency;
    }

    void FollowPlayer()
    {
        if (!collectionToFollow) return;
        var firstPlayer = collectionToFollow.GetFirstElement();
        if (!firstPlayer) return;
        transform.position = firstPlayer.transform.position;
    }

    void CalculateSpawnFrequency()
    {
        if (!darknessLevel) return;
        if (InDarkness())
        {
            _spawnFrequency += spawnFrequencyIncreaseSpeed.Value * Time.deltaTime;
            _spawnFrequency = Mathf.Clamp(_spawnFrequency, minSpawnFrequency, maxSpawnFrequency);
        }
        else _spawnFrequency = 0;
    }

    bool InDarkness()
    {
        if (!darknessLevel) return false;
        return darknessLevel.Value >= .98f;
    }
    
    bool InFullLight()
    {
        if (!darknessLevel) return false;
        return darknessLevel.Value <= .8f;
    }
}
