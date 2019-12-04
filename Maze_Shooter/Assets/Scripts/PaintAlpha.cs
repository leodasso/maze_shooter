using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// This script is intended to be a way for prefab painter to have access to painting different alphas.
/// Prefab painter simply sets an alpha value, and this component handles what renderers are affected.
/// </summary>
public class PaintAlpha : MonoBehaviour
{
    [Range(0, 1), OnValueChanged("ApplyAlpha")]
    public float alpha = 1;
    
    public List<SpriteRenderer> sprites = new List<SpriteRenderer>();

    public void SetAlpha(float newAlpha)
    {
        alpha = newAlpha;
        ApplyAlpha();
    }

    void ApplyAlpha()
    {
        foreach (SpriteRenderer sprite in sprites)
        {
            Color c = sprite.color;
            sprite.color = new Color(c.r, c.g, c.b, alpha);
        }
    }
}
