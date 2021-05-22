using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpriteFlipper : MonoBehaviour
{
	public SpriteRenderer spriteRenderer;
	public bool randomFlipX;
	public bool randomFlipY;

    // Start is called before the first frame update
    void Start()
    {
        if (randomFlipX) spriteRenderer.flipX = CoinFlip;
		if(randomFlipY) spriteRenderer.flipY = CoinFlip;
    }

	bool CoinFlip => Random.Range(0f, 1f) > .5f;
}
