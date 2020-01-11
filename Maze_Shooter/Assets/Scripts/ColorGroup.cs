using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ColorGroup : MonoBehaviour
{
    [OnValueChanged("SetColor")]
    public Color color = Color.white;
    public bool overwriteAlpha;
    public List<SpriteRenderer> sprites = new List<SpriteRenderer>();
    float _tempAlpha;

    [ButtonGroup()]
    public void SetColor()
    {
        GetSprites();
        foreach (var sprite in sprites)
        {
            _tempAlpha = overwriteAlpha ? color.a : sprite.color.a;
            sprite.color = new Color(color.r, color.g, color.b, _tempAlpha);
        }
    }

    [ButtonGroup()]
    public void GetSprites()
    {
        sprites.Clear();
        sprites.AddRange(GetComponentsInChildren<SpriteRenderer>());
    }
}