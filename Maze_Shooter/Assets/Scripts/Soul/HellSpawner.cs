using System.Collections.Generic;
using System.Collections;
using System.Linq;
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
    
    [Tooltip("Ref to the float value which holds the darkness value.")]
    public FloatValue darknessLevel;
    
    [Tooltip("Radius around the player that evil eyes can be spawned.")]
    public float spawnRadius = 10;
    
    [Tooltip("How long to wait from when the player leaves the light to spawning the first evil eyes")]
    public float spawnDelay = 1;

    [Tooltip("When the player re-enters the light, the evil eye instances are destroyed one at a time. " +
             "This is how long in seconds it takes to destroy all of them.")]
    public float destroyTime = .5f;
    
    [Tooltip("The min scale of a spawned evil eye.")]
    [BoxGroup("scale", GroupName = "Spawn Scale")]
    [HorizontalGroup("scale/minMax"), LabelText("Min"), LabelWidth(60)]
    public float minSpawnScale = .7f;
    
    [Tooltip("The max scale of a spawned evil eye.")]
    [HorizontalGroup("scale/minMax"), LabelText("Max"), LabelWidth(60)]
    public float maxSpawnScale = 1.2f;
    
    [BoxGroup("freq", GroupName = "Spawn Frequency"), LabelText("Increase Speed")]
    public FloatReference spawnFrequencyIncreaseSpeed;
    
    [HorizontalGroup("freq/minMax"), LabelText("Min"), LabelWidth(60)]
    public float minSpawnFrequency;
    
    [HorizontalGroup("freq/minMax"), LabelText("Max"), LabelWidth(60)]
    public float maxSpawnFrequency;
    
    [ShowInInspector, ReadOnly, BoxGroup("freq")]
    float _spawnFrequency;
    
    // Evil eye instances that have been spawned in
    List<GameObject> _instances = new List<GameObject>();

    GameObject _hellSpawnParent;

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
            spawnDelay -= Time.deltaTime;
            if (spawnDelay <= 0)
                Spawn();
        }

        // When the player returns to fully lit areas,
        // reset the spawn delay so the eye's don't immediately start appearing when they enter darkness.
        if (InFullLight())
        {
            spawnDelay = 1;
            DestroyDemons();
        }
    }

    /// <summary>
    /// Sequentially destroys all the demons that have been spawned while in the dark.
    /// </summary>
    void DestroyDemons()
    {
        if (_instances.Count <= 0) return;
        StartCoroutine(DestroyDemonsSequence());
    }

    IEnumerator DestroyDemonsSequence()
    {
        // Order the spawned demons by their distance to the spawner.
        // That way we can destroy the ones closer to the spawner(and therefore the light)
        // first, and it will look cool.
        List<GameObject> instancesToDestroy = 
            _instances.OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).ToList();
        _instances.Clear();

        int count = instancesToDestroy.Count;
        float waitTime = destroyTime / count;
        
        foreach (var demon in instancesToDestroy)
        {
            Destroy(demon);
            yield return new WaitForSeconds(waitTime);
        }
    }

    void Spawn()
    {
        // Failsafe so we never spawn evil eyes in the light
        if (!InDarkness()) return;

        if (!_hellSpawnParent)
            _hellSpawnParent = new GameObject("Hell Spawns");

        // Pick the prefab and a random location, then spawn an instance
        var toSpawn = Math.RandomElementOfList(evilEyesPrefabs);
        Vector3 pos = Random.onUnitSphere * spawnRadius;
        var newInstance = Instantiate(toSpawn, pos + transform.position, quaternion.identity);
        newInstance.transform.parent = _hellSpawnParent.transform;
        newInstance.transform.localScale *= Random.Range(minSpawnScale, maxSpawnScale);
        _instances.Add(newInstance);
        
        // Setup for the next spawn
        spawnDelay = 1 / _spawnFrequency;
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
