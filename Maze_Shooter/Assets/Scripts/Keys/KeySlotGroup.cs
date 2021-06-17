using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class KeySlotGroup : MonoBehaviour
{
    public List<KeySlot> keySlots = new List<KeySlot>();

    public SavedBool keySlotsFilled;
    public UnityEvent onFirstTimeActivated;
    public UnityEvent onLoadAsActivated;

    bool _alreadyActivated;

    void Awake()
    {
        foreach (var keySlot in keySlots)
        {
            if (!keySlot) continue;
            keySlot.onSlotFilled += CheckMyKeySlots;
        }
    }

    void Start()
    {
        if (keySlotsFilled && keySlotsFilled.runtimeValue)
        {
            onLoadAsActivated.Invoke();
            _alreadyActivated = true;
        }
    }
    
    [Button]
    void GetChildKeySlots()
    {
        keySlots.Clear();
        keySlots.AddRange(GetComponentsInChildren<KeySlot>());
    }

    void CheckMyKeySlots()
    {
        if (_alreadyActivated) return;

        foreach (var keySlot in keySlots)
        {
            if (!keySlot.IsFilled) return;
        }
        Activate();
    }

    void Activate()
    {
        _alreadyActivated = true;
        onFirstTimeActivated.Invoke();
        if (keySlotsFilled) keySlotsFilled.runtimeValue = true;
    }
}
