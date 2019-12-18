﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Ghost/Constellation Data")]
public class ConstellationData : ScriptableObject
{
    public string title = "Please name me";

    public bool HasBeenCollected()
    {
        // TODO
        return false;
    }

    public void SaveAsCollected()
    {
        // TODO
    }
}
