using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimVolume : Volume
{
    public SpriteRenderer spriteRenderer;
    public List<Sprite> sprites = new List<Sprite>();
    
    public override void ApplyVolume(float newValue)
    {
        newValue = Mathf.Clamp01(volumeCurve.Evaluate(newValue));
        int spriteIndex = Mathf.FloorToInt(newValue * sprites.Count);
        if (spriteIndex >= sprites.Count) 
            spriteIndex = sprites.Count - 1;
        spriteRenderer.sprite = sprites[spriteIndex];
    }
}
