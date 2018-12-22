using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class StageDescriptor : MonoBehaviour
{
	public Stage stage;
	public TextMeshProUGUI title;
	public CanvasGroup canvasGroup;
	public Image background;
	float _alpha;

	// Use this for initialization
	void Awake()
	{
		canvasGroup.alpha = 0;
		_alpha = 0;
		Refresh();
	}

	void Update()
	{
		canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, _alpha, Time.unscaledDeltaTime * 10);
	}

	public void Refresh()
	{
		title.text = stage.displayName;
		background.color = stage.stageColor;
	}

	public void Show()
	{
		_alpha = 1;
	}

	public void Hide()
	{
		_alpha = 0;
	}
}
