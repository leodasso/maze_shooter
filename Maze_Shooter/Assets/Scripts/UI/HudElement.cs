﻿using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class HudElement : MonoBehaviour
{
	[SerializeField]
	float showTime = 5;

	[ShowInInspector, ReadOnly]
	bool isShowing;

	public UnityEvent show;
	public UnityEvent hide;

	float showTimer = 0;


    // Start is called before the first frame update
    void Start()
    {
    }

	void Update() 
	{
		if (showTimer > 0) 
			showTimer -= Time.unscaledDeltaTime;

		if (showTimer <= 0 && isShowing) 
			Hide();
	}

    public void Show() 
	{
		if (isShowing) return;
		isShowing = true;
		showTimer = showTime;
		show.Invoke();
	}

	public void Hide() 
	{
		if (!isShowing) return;
		isShowing = false;
		hide.Invoke();
	}
}