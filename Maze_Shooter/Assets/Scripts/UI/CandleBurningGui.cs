using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleBurningGui : MonoBehaviour
{
	[SerializeField]
    CanvasGroupHelper mainGroup;

	public void SetVisible(bool isVisible)
	{
		mainGroup.SetAlpha(isVisible ? 1 : 0);
	}
}
