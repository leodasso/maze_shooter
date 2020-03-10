using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;

public class ColorGroup : MonoBehaviour
{
    public bool controlColor = true;

    [ShowIf("controlColor"), Tooltip("Apply the color to sprites on every update frame")]
    public bool updateColor;
    
    [OnValueChanged("SetColor"), ShowIf("controlColor")]
    public Color color = Color.white;
    
    [Tooltip("Should the alpha of the above color also be applied to all sprites?"), ShowIf("controlColor")]
    public bool overwriteAlpha;

    [Space]
    public bool controlAlpha;
    
    [ShowIf("controlAlpha"), Tooltip("Apply the alpha to sprites on every update frame")]
    public bool updateAlpha;
    
    [Range(0, 1), ShowIf("controlAlpha"), OnValueChanged("SetAlpha")]
    public float alpha = 1;
    
    public List<SpriteRenderer> sprites = new List<SpriteRenderer>();
    public List<TextMeshPro> texts = new List<TextMeshPro>();
    public List<ColorGroup> childColorGroups = new List<ColorGroup>();

    [Tooltip("Automatically gets sprites and color groups from children when updating color/alpha in editor.")]
    public bool autoGetSprites = true;
    float _tempAlpha;

    void Update()
    {
        if (controlColor && updateColor)
            SetColorFast(Color.white);
        
        if (controlAlpha && updateAlpha)
            SetAlphaFast();
    }

    [ButtonGroup()]
    void SetColor()
    {
        if (!controlColor) return;
        if (autoGetSprites)
        {
            GetChildColorGroups();
            GetSprites();
        }

        SetColorFast(Color.white);
    }

    /// <summary>
    /// Doesn't refresh sprites list - can be called in Update
    /// </summary>
    public void SetColorFast(Color parentColor)
    {
        if (!controlColor) return;

        Color finalColor = color * parentColor;
        
        foreach (var sprite in sprites)
        {
            _tempAlpha = overwriteAlpha ? finalColor.a : sprite.color.a;
            sprite.color = new Color(finalColor.r, finalColor.g, finalColor.b, _tempAlpha);
        }
        
        foreach (var textMesh in texts)
        {
            _tempAlpha = overwriteAlpha ? finalColor.a : textMesh.color.a;
            textMesh.color = new Color(finalColor.r, finalColor.g, finalColor.b, _tempAlpha);
        }
        
        UpdateChildColorGroups();
    }

    public void SetAlphaTo(float newAlpha)
    {
        if (!controlAlpha) return;
        alpha = Mathf.Clamp01(newAlpha);
        
        SetAlphaFast();
    }

    /// <summary>
    /// Sets the alpha after refreshing color group and sprites list. This is slow
    /// and should only be used by the inspector.
    /// </summary>
    void SetAlpha()
    {
        if (!controlAlpha) return;
        if (autoGetSprites)
        {
            GetChildColorGroups();
            GetSprites();
        }
        SetAlphaFast();
    }

    /// <summary>
    /// Doesn't refresh sprites list - can be called in Update
    /// </summary>
    void SetAlphaFast(float parentAlpha = 1)
    {
        if (!controlAlpha) return;
        foreach (var sprite in sprites)
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha * parentAlpha);

        foreach (var textMesh in texts)
            textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, alpha * parentAlpha);
        
        UpdateChildColorGroups();
    }

    void UpdateChildColorGroups()
    {
        foreach (var childGroup in childColorGroups)
        {
            childGroup.SetAlphaFast(alpha);
            childGroup.SetColorFast(color);
        }
    }

    /// <summary>
    /// Gets only immediate children color groups, not all descendents
    /// </summary>
    void GetChildColorGroups()
    {
        childColorGroups.Clear();
        foreach (var colorGroup in GetComponentsInChildren<ColorGroup>())
        {
            if (colorGroup == this) continue;
            
            // Choosing the 1 index because the 0 index of getComponentsInParent is always the object itself.
            ColorGroup parentColorGroup = colorGroup.GetComponentsInParent<ColorGroup>()[1];
            if (parentColorGroup != this) continue;
            childColorGroups.Add(colorGroup);
        }
    }

    [ButtonGroup()]
    public void GetSprites()
    {
        sprites.Clear();
        sprites.AddRange(GetComponentsInChildren<SpriteRenderer>());
        texts.Clear();
        texts.AddRange(GetComponentsInChildren<TextMeshPro>());
    }
}