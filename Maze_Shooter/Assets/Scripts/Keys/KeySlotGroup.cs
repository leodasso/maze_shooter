using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using Arachnid;

public class KeySlotGroup : MonoBehaviour
{
    public List<KeySlot> keySlots = new List<KeySlot>();

    public BoolValue keySlotsFilled;
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
        if (keySlotsFilled && keySlotsFilled.Value)
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
        if (keySlotsFilled) keySlotsFilled.Value = true;
    }
}
