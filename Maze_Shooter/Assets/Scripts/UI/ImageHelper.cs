using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageHelper : MonoBehaviour
{
	Image _image;

	// Use this for initialization
	void Awake()
	{
		_image = GetComponent<Image>();
	}

	public void SetColor(Color newColor)
	{
		_image.color = newColor;
	}

	public void AdjustAlpha(float alphaDelta)
	{
		float alpha = _image.color.a;
		alpha += alphaDelta;
		alpha = Mathf.Clamp01(alpha);
		SetAlpha(alpha);
	}

	public void SetAlpha(float newAlpha)
	{
		_image.color = new Color(_image.color.r, _image.color.g, _image.color.b, newAlpha);
	}
}
