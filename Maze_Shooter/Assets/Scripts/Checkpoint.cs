using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

[TypeInfoBox("Checkpoints will spawn the player to them when the stage loads. IMPORTANT: In order for " +
             "checkpoints to work, the scene has to have a Stage (scriptableObject) associated with it. That " +
             "stage has to be the gameMaster's current stage as well. ")]
public class Checkpoint : MonoBehaviour
{
    [Tooltip("The ID for this checkpoint. Must be unique from any other checkpoints in this scene, but it's fine" +
             " if it's the same as a checkpoint in a different scene.")]
    public string uniqueId = "ENTER A UNIQUE ID";

    [Tooltip("The collection of player spawnpoints")]
    public Collection spawnPointsCollection;

    public UnityEvent onDeactivated;

    void Awake()
    {
        if (!IsActiveCheckpoint()) return;
        
        foreach (var sp in spawnPointsCollection.elements)
            sp.transform.position = transform.position;
    }

    bool IsActiveCheckpoint()
    {
        // TODO
        return false;
        //if (GameMaster.Get().currentStage == null) return false;
        //return GameMaster.Get().currentStage.CheckpointIsActive(uniqueId);
    }

    [Button]
    public void SetAsActiveCheckpoint()
    {
        //if (GameMaster.Get().currentStage == null) return;
        //GameMaster.Get().currentStage.SetActiveCheckpoint(uniqueId);
    }

    /// <summary>
    /// Call this when another checkpoint was activated. Intended to be called via UnityEvent or other editor thing.
    /// </summary>
    public void OtherCheckpointWasActivated()
    {
        if (!IsActiveCheckpoint())
            onDeactivated.Invoke();
    }
}
