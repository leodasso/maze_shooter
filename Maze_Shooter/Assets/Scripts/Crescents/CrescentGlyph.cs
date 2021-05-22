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

	[Tooltip("Not the actual facing of the transform, but the perceived facing direction of the graphic")]
	public Vector3 facingDirection = new Vector3(1, 0, 1);

	void OnDrawGizmosSelected()
	{	
		Gizmos.color = Color.yellow;
		Gizmos.DrawRay(transform.position, facingDirection.normalized);
	}


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

	[ButtonGroup]
	void FaceDownRight()
	{
		facingDirection = new Vector3(1, 0, -1);
	}

	[ButtonGroup]
	void FaceDownLeft()
	{
		facingDirection = new Vector3(-1, 0, -1);
	}

	[ButtonGroup]
	void FaceUp()
	{
		facingDirection = new Vector3(0, 1, 0);
	}
}
