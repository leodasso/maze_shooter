using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu]
public class TextFormattingData : ScriptableObject
{
    public List<TextFormattingSet> formattingSets = new List<TextFormattingSet>();
}

[System.Serializable]
public struct TextFormattingSet
{
    [HorizontalGroup("pair", Title = "Hidden / Opener / Closer"), HideLabel]
    public string opener;
    [HorizontalGroup("pair"), HideLabel]
    public string closer;
}
