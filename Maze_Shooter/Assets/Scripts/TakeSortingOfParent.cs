using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer)), ExecuteInEditMode]
public class TakeSortingOfParent : MonoBehaviour
{
	public int offset;
	SpriteRenderer _parentSpriteRenderer;
	SpriteRenderer _spriteRenderer;
	
	// Use this for initialization
	void Start ()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_parentSpriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
		Execute();
	}

	void Update()
	{
		Execute();
	}

	void Execute()
	{
		if (!_spriteRenderer || !_parentSpriteRenderer) return;
		_spriteRenderer.sortingLayerID = _parentSpriteRenderer.sortingLayerID;
		_spriteRenderer.sortingOrder = _parentSpriteRenderer.sortingOrder + offset;
	}
}