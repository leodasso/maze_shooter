﻿using System;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

[RequireComponent(typeof(GuidGenerator))]
public class KeySlot : MonoBehaviour
{
    public GuidGenerator guidGenerator;
    public UnityEvent onKeyInserted;
    public UnityEvent onInsertInstantly;
    public bool IsFilled => _filled;
    bool _filled;

    public Action onSlotFilled;
    

	[ButtonGroup]
    public void InsertKey()
    {
        onKeyInserted.Invoke();
        FillSlot();
    }

	[ButtonGroup]
    public void InsertKeyInstantly()
    {
        onInsertInstantly.Invoke();
        FillSlot();
    }

    void FillSlot()
    {
        _filled = true;
        if (onSlotFilled != null) 
            onSlotFilled.Invoke();
    }
}
