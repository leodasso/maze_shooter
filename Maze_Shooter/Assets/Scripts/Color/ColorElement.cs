using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// This component goes on an object/prefab, and interfaces with the color manager of the current scene
/// to make sure the colors of this object adhere to the scene's color profile.
/// </summary>
public class ColorElement : MonoBehaviour
{
    public ColorCategory colorCategory = ColorCategory.Primary;
    public List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
    public List<ColorGroup> colorGroups = new List<ColorGroup>();

    [Button]
    void AutoPopulate()
    {
        spriteRenderers.Clear();
        spriteRenderers.AddRange(GetComponentsInChildren<SpriteRenderer>());
        
        colorGroups.Clear();
        colorGroups.AddRange(GetComponentsInChildren<ColorGroup>());
    }

    
    public void ApplyColorProfile(ColorProfile profile)
    {
        Color newColor = profile.GetColor(colorCategory);
        
        foreach (SpriteRenderer sr in spriteRenderers)
            sr.color = newColor;

        foreach (var colorGroup in colorGroups)
        {
            colorGroup.color = newColor;
            colorGroup.SetColorFast(Color.white);
        }
    }
}
