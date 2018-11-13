using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteRotation : MonoBehaviour
{
	public float Rotation
	{
		get { return _rotation; }
		set { _rotation = Math.Angle0to360(value + rotationOffset); }
	}

	[Range(-180, 180)]
	public float rotationOffset;
	
	[Range(0, 360), OnValueChanged("UpdateSprite"), SerializeField, ShowInInspector]
	float _rotation;
	
	public List<Sprite> sprites = new List<Sprite>();

	SpriteRenderer _spriteRenderer;

	// Use this for initialization
	void Start ()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		UpdateSprite();
	}


	void UpdateSprite()
	{
		if (!_spriteRenderer) _spriteRenderer = GetComponent<SpriteRenderer>();
		float r = _rotation / 360f;
		r *= (sprites.Count - 1);

		_spriteRenderer.sprite = sprites[Mathf.RoundToInt(r)];
	}
}
