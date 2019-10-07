using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(SpriteRenderer)), ExecuteAlways]
public class SpriteRotation : MonoBehaviour
{
	[ShowInInspector, OnValueChanged("UpdateSprite"), PropertyRange(0, 360)]
	public float Rotation
	{
		get { return _rawRotation; }
		set
		{
			_rawRotation = value;
			_rotation = Math.Angle0to360(value + rotationOffset);
		}
	}

	[ToggleLeft]
	public bool useRotationOfObject;

	[ShowIf("useRotationOfObject"), Tooltip("The rotation of this object will be used to select which sprite to show.")]
	public GameObject objectToUse;

	[Range(-180, 180), OnValueChanged("UpdateRotationOffset")]
	public float rotationOffset;

	float _rawRotation;
	float _rotation;
	
	[PreviewField]
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
		if (useRotationOfObject && objectToUse) Rotation = objectToUse.transform.eulerAngles.z;
		UpdateSprite();
	}

	void UpdateRotationOffset()
	{
		_rotation = Math.Angle0to360(Rotation + rotationOffset);
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
