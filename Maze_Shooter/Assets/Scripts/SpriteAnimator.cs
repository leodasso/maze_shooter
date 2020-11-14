using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SpriteAnimator : MonoBehaviour
{
	[Range(0, 1)]
	public float progress = 0;
    public SpriteRenderer spriteRenderer;
    public List<Sprite> sprites = new List<Sprite>();
    
	public void Update() 
	{
		if (!spriteRenderer) return;
		if (sprites.Count < 1) return;
        int spriteIndex = Mathf.FloorToInt(progress * sprites.Count);
        if (spriteIndex >= sprites.Count) 
            spriteIndex = sprites.Count - 1;
        spriteRenderer.sprite = sprites[spriteIndex];
	}

}
