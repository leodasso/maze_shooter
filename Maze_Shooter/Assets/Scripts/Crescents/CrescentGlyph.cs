using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class CrescentGlyph : MonoBehaviour
{
	[SerializeField]
	SpriteRenderer spriteRenderer;

	Sprite crescentSprite;

	[SerializeField, PreviewField]
	Sprite crescentEmptySprite;

	[SerializeField]
	UnityEvent onActivate;


    // Start is called before the first frame update
    void Start()
    {
        crescentSprite = spriteRenderer.sprite;
		Deactivate();
    }


	public void Activate() 
	{
		spriteRenderer.sprite = crescentSprite;
		onActivate.Invoke();
	}

	public void Deactivate() 
	{
		spriteRenderer.sprite = crescentEmptySprite;
	}
}
