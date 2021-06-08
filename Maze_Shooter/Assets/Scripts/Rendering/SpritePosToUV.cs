using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePosToUV : MonoBehaviour
{
	public SpriteRenderer spriteRenderer;
	public float scaleFactor = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (!spriteRenderer) return;
		if (spriteRenderer.drawMode != SpriteDrawMode.Tiled) return;

		spriteRenderer.material.mainTextureOffset = new Vector2(
			transform.position.x * scaleFactor, 
			transform.position.z * scaleFactor);
    }
}
