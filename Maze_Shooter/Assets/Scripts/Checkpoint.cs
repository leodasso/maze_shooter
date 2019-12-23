﻿using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

[TypeInfoBox("Checkpoints will spawn the player to them when the stage loads.")]
public class Checkpoint : MonoBehaviour
{
    [Tooltip("The ID for this checkpoint. Must be unique from any other checkpoints in this scene, but it's fine" +
             " if it's the same as a checkpoint in a different scene.")]
    public string uniqueId = "ENTER A UNIQUE ID";
    
    public UnityEvent onDeactivated;

    [SerializeField, ShowInInspector]
    UnityEvent _onSpawn;
    
    bool IsActiveCheckpoint => GameMaster.IsCurrentCheckpoint(uniqueId);

    public void SpawnPlayer()
    {
        _onSpawn.Invoke();
    }

    
    public void SpawnIfCurrentCheckpoint()
    {
        if (IsActiveCheckpoint)
            SpawnPlayer();
    }

    
    [Button]
    public void SetAsActiveCheckpoint()
    {
        GameMaster.SetCheckpoint(uniqueId);
    }

    
    /// <summary>
    /// Call this when another checkpoint was activated. Intended to be called via UnityEvent or other editor thing.
    /// </summary>
    public void OtherCheckpointWasActivated()
    {
        if (!IsActiveCheckpoint)
            onDeactivated.Invoke();
    }
}
