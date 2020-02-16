using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(GuidGenerator))]
public class Key : MonoBehaviour
{
    public GuidGenerator guidGenerator;
    public UnityEvent onAqcuired;
    public UnityEvent onFirstTimeAcquire;
    public UnityEvent onUsed;
    GameObject _beholder;
    bool _acquired;
    const string saveDataPrefix = "key_";

    // Start is called before the first frame update
    void Start()
    {
        // if this key is already acquired, move to the beholder
        if (LoadAcquiredStatus()) Acquire();
    }

    public void Acquire()
    {
        _acquired = true;
        onAqcuired.Invoke();
        onFirstTimeAcquire.Invoke();
        Debug.Log("key was acquired!");
    }

    bool LoadAcquiredStatus()
    {
        if (!guidGenerator) return false;
        return GameMaster.LoadFromCurrentFile(saveDataPrefix + guidGenerator.uniqueId, false, this);
    }
}
