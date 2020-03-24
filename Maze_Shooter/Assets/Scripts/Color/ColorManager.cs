using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    [OnValueChanged("RefreshAllColorElements")]
    public ColorProfile colorProfile;
    List<ColorElement> _colorElements = new List<ColorElement>();

    /// <summary>
    /// This is very expensive and is EDITOR ONLY! Do not use this during gameplay
    /// </summary>
    [Button]
    void GetAllColorElements()
    {
        _colorElements.Clear();
        _colorElements.AddRange(FindObjectsOfType<ColorElement>());
        Debug.Log("Found " + _colorElements.Count + " color elements in the scene.");
    }

    [Button]
    void RefreshAllColorElements()
    {
        if (!colorProfile) return;
        foreach (ColorElement element in _colorElements)
            element.ApplyColorProfile(colorProfile);
    }
}
