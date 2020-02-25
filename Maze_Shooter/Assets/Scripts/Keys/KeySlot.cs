using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(GuidGenerator))]
public class KeySlot : MonoBehaviour
{
    public GuidGenerator guidGenerator;
    public UnityEvent onKeyInserted;
    public UnityEvent onInsertInstantly;
    public bool IsFilled => _filled;
    bool _filled;
    

    public void InsertKey()
    {
        _filled = true;
        onKeyInserted.Invoke();
    }

    public void InsertKeyInstantly()
    {
        _filled = true;
        onInsertInstantly.Invoke();
    }
}
