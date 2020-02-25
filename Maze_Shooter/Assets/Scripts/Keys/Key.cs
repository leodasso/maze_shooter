using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(GuidGenerator))]
public class Key : MonoBehaviour
{
    public enum KeyState
    {
        Idle,
        Acquired,
        InSlot,
    }
    
    [Tooltip("With debug mode on, this key will always behave as if it's being gotten for the first time.")]
    public bool debugMode;

    public KeyState keyState = KeyState.Idle;
    
    public Orbiter orbiter;
    public GuidGenerator guidGenerator;
    [Tooltip("When the player first acquires the keystone, this happens. Use to show the cool blingy feedback of " +
             "'you got the thing yay'")]
    public UnityEvent onAqcuired;
    
    [Tooltip("If the player has previously acquired this keystone but is returning to the scene, this action happens.")]
    public UnityEvent moveToBeholder;
    
    public UnityEvent fadeOut;
    GameObject _beholder;
    const string saveDataPrefix = "key_";
    const string slotNameSaveDataSuffix = "_keySlot";
    string _keySlot;

    
    // Start is called before the first frame update
    void Start()
    {
        _keySlot = LoadSlot();
        if (_keySlot != "none")
        {
            keyState = KeyState.InSlot;
            PlaceKeyInSavedSlotInstantly();
        }
        
        else if (LoadAcquiredStatus())
        {
            keyState = KeyState.Acquired;
            StartCoroutine(MoveToBeholder());
        }
    }

    // When trying to orbit around the player on start, sometimes the player hasn't been spawned yet.
    // This will wait for the player to spawn before invoking 'orbitPlayer'
    IEnumerator MoveToBeholder()
    {
        while (GameMaster.GetPlayerInstance() == null)
        {
            yield return new WaitForSecondsRealtime(.1f);
        }

        var keyGrabber = GameMaster.GetPlayerInstance().GetComponentInChildren<KeyGrabber>();
        keyGrabber.GrabKey(gameObject);
        
        moveToBeholder.Invoke();
    }

    // This function will be called by UnityEvents
    public void Acquire()
    {
        if (!CanBeAcquired()) return;

        keyState = KeyState.Acquired;
        SaveAsAcquired();
        onAqcuired.Invoke();
    }

    void PlaceKeyInSavedSlotInstantly()
    {
        var keySlot = GetSavedKeySlot();
        if (!keySlot) return;
        keySlot.InsertKeyInstantly();
        RemoveMe();
    }

    KeySlot GetSavedKeySlot()
    {
        GameObject savedSlotGameObject = GuidGenerator.GetObjectForGuid(LoadSlot());
        if (!savedSlotGameObject) return null;
        return savedSlotGameObject.GetComponent<KeySlot>();
    }

    bool CanBeAcquired()
    {
#if UNITY_EDITOR
        if (debugMode) return true;
#endif
        return keyState == KeyState.Idle;
    }

    public void PlaceIntoSlot(GameObject keySlot)
    {
        // Lets us turn off the component to prevent duplicate placements
        if (!enabled) return;
        
        KeySlot slot = keySlot.GetComponent<KeySlot>();
        if (!slot)
        {
            Debug.LogError("No KeySlot component was found on " + keySlot.name, keySlot);
            return;
        }

        if (slot.IsFilled) return;
        
        slot.InsertKey();
        SaveKeySlot(slot.guidGenerator.uniqueId);
        RemoveMe();
    }

    void RemoveMe()
    {
        fadeOut.Invoke();
        Destroy(gameObject, 1);
        enabled = false;
    }

    /// <summary>
    /// Returns the GUID of the slot that this key has been placed in. If it hasn't yet been placed in a
    /// slot, returns 'none'
    /// </summary>
    string LoadSlot()
    {
        if (!guidGenerator) return "none";
        return GameMaster.LoadFromCurrentFile(
            saveDataPrefix + guidGenerator.uniqueId + slotNameSaveDataSuffix, "none", this);
    }

    /// <summary>
    /// Saves the keyslot that this key has been placed into.
    /// </summary>
    void SaveKeySlot(string slotGuid)
    {
        if (!guidGenerator)
        {
            Debug.LogError("Trying to save slot for keystone, but no guid exists! Please add a GUID generator" +
                           " component.", gameObject);
            return;
        }
        
        GameMaster.SaveToCurrentFile(
            saveDataPrefix + guidGenerator.uniqueId + slotNameSaveDataSuffix, slotGuid, this);
    }
    

    /// <summary>
    /// Does the save file show that this has been acquired previously?
    /// </summary>
    bool LoadAcquiredStatus()
    {
#if UNITY_EDITOR
        if (debugMode) return false;
#endif
        if (!guidGenerator) return false;
        return GameMaster.LoadFromCurrentFile(saveDataPrefix + guidGenerator.uniqueId, false, this);
    }
    

    void SaveAsAcquired()
    {
        if (!guidGenerator)
        {
            Debug.LogError("Trying to save for keystone, but no guid exists! Please add a GUID generator" +
                             " component.", gameObject);
            return;
        }
        GameMaster.SaveToCurrentFile(saveDataPrefix + guidGenerator.uniqueId, true, this);
    }
    
    public void DisableAnimator()
    {
        var animator = GetComponent<Animator>();
        if (animator) animator.enabled = false;
    }

    public void EnableAnimator()
    {
        var animator = GetComponent<Animator>();
        if (animator) animator.enabled = true;
    }
}
