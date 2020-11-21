using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(GuidGenerator))]
public class Key : MonoBehaviour
{
    
    [Tooltip("With debug mode on, this key will always behave as if it's being gotten for the first time.")]
    public bool debugMode;
    
    public GuidGenerator guidGenerator;
    
    public UnityEvent onUseKey;
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
            PlaceKeyInSavedSlotInstantly();
        }
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
        onUseKey.Invoke();
        Destroy(gameObject);
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
