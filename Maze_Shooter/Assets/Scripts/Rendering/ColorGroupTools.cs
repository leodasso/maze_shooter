using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGroupTools : MonoBehaviour
{
	public float fadeDuration = 2;
	public ColorGroup colorGroup;

	[Tooltip("Place any colors used in fading here. When you call FadeToColor(index goes here) ")]
	public List<Color> colors = new List<Color>();

	public void FadeToColor(int colorIndex) {
		if (!colorGroup) return;
		StartCoroutine(FadeToColorRoutine(colors[colorIndex], fadeDuration));
	}

	IEnumerator FadeToColorRoutine(Color newColor, float duration) {

		float progress = 0;
		Color startColor = colorGroup.color;
		
		while (progress < 1) {
			progress += Time.deltaTime / duration;
			colorGroup.color = Color.Lerp(startColor, newColor, progress);
			yield return null;
		}
		colorGroup.color = newColor;
	}

	public void FadeToAlpha(float newAlpha) {
		if (!colorGroup) return;
		StartCoroutine(FadeToAlphaRoutine(newAlpha, fadeDuration));
	}

	IEnumerator FadeToAlphaRoutine(float newAlpha, float duration) {

		float progress = 0;
		float startAlpha = colorGroup.alpha;
		
		while (progress < 1) {
			progress += Time.deltaTime / duration;
			colorGroup.alpha = Mathf.Lerp(startAlpha, newAlpha, progress);
			yield return null;
		}
		colorGroup.alpha = newAlpha;
	}
}
