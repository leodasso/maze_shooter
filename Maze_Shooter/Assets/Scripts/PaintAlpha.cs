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
    
    public List<AlphaSet> sprites = new List<AlphaSet>();

    public void SetAlpha(float newAlpha)
    {
        alpha = newAlpha;
        ApplyAlpha();
    }

    void ApplyAlpha()
    {
        foreach (AlphaSet set in sprites)
        {
            set.Apply(alpha);
        }
    }

    [System.Serializable]
    public class AlphaSet
    {
        [Range(0, 1)]
        public float alpha = 1;
        public SpriteRenderer spriteRenderer;

        public void Apply(float parentAlpha)
        {
            Color c = spriteRenderer.color;
            spriteRenderer.color = new Color(c.r, c.g, c.b, alpha * parentAlpha);
        }
    }
}
