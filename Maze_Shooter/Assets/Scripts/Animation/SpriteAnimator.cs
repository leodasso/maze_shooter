﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

[ExecuteAlways]
public class SpriteAnimator : MonoBehaviour
{
	[Range(0, 1)]
	public float progress = 0;
    public SpriteRenderer spriteRenderer;
    public List<Sprite> sprites = new List<Sprite>();

	[Space]
	public bool unscaledTime;
	public bool playSelf;

	[ShowIf("playSelf")]
	public bool pingPong;

	[ShowIf("playSelf")]
	public bool loop = true;

	[ShowIf("playSelf")]
	public float frameRate = 12;

	public bool setProgressOnStart;
	[Range(0, 1), ShowIf("setProgressOnStart")]
	public float startProgress;

	public List<SpriteAnimEvent> spriteAnimEvents = new List<SpriteAnimEvent>();

	int playDirection = 1;

	// how many times has the full animation played? 
	int _plays = 0;

	float DeltaTime => unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

	void Start() {
		Reset();
		if (setProgressOnStart) progress = startProgress;
	}

	public void Play(float duration) {
		StartCoroutine(DoPlay(duration));
	}

	IEnumerator DoPlay(float duration) {
		while (progress < 1) {
			progress += DeltaTime / duration;
			yield return null;
		}
	}
    
	public void Update() 
	{
		if (!spriteRenderer) return;
		if (sprites.Count < 1) return;
		if (!IsPlaying) return;

		int spriteIndex = Mathf.FloorToInt(progress * sprites.Count);
        if (spriteIndex >= sprites.Count) 
            spriteIndex = sprites.Count - 1;
        spriteRenderer.sprite = sprites[spriteIndex];

		if (playSelf) SelfPlayUpdate();

		foreach (var animEvent in spriteAnimEvents)
			animEvent.ProcessProgress(progress);
	}

	bool IsPlaying => loop || _plays < 1;

	void SelfPlayUpdate() 
	{
		float progressRate = frameRate / sprites.Count;
		progress += DeltaTime * progressRate * playDirection;

		if (progress >= 1) {
			if (pingPong) playDirection = -1;
			else progress = 0;
		}

		else if (progress <= 0) {
			if (pingPong) playDirection = 1;
			else progress = 1;
		}

		progress = Mathf.Clamp01(progress);
		if (progress == 0) _plays++;

	}

	[Button]
	public void Reset() {  
		_plays = 0;
		progress = 0;
	}

	[System.Serializable]
	public class SpriteAnimEvent 
	{
		public UnityEvent onAnimEvent;

		[Tooltip("Event can only be fired once per object lifetime"), ToggleLeft, SerializeField]
		bool oneTime = true;

		[Tooltip("If the animation progress is within this range, fire the event"), MinMaxSlider(0, 1), SerializeField]
		Vector2 progressRange = new Vector2(0.9f, 1);
		bool animEventFired;

		bool IsInRange(float newProgress) {
			return newProgress >= progressRange.x && newProgress <= progressRange.y;
		}

		public void ProcessProgress(float newProgress) 
		{
			if (IsInRange(newProgress) && !animEventFired) {
				animEventFired = true;
				onAnimEvent.Invoke();
				return;
			}
			
			// if this isn't a one time event, reset the trigger
			if (!oneTime && !IsInRange(newProgress))
				animEventFired = false;
		}
	}
}