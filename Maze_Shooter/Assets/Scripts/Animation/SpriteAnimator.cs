using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Sirenix.OdinInspector;

[ExecuteAlways]
public class SpriteAnimator : MonoBehaviour
{
	[Range(0, 1)]
	public float progress = 0;
    public SpriteRenderer spriteRenderer;
	public Image image;
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

	[SerializeField]
	bool resetOnEnable;
	
	[SerializeField]
	bool resetOnStart = true;

	public bool setProgressOnStart;
	[Range(0, 1), ShowIf("setProgressOnStart")]
	public float startProgress;

	public List<SpriteAnimEvent> spriteAnimEvents = new List<SpriteAnimEvent>();

	int playDirection = 1;

	// how many times has the full animation played? 
	int _plays = 0;

	float DeltaTime => unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

	void Start() 
	{
		if (resetOnStart)
			Reset();

		if (setProgressOnStart) 
			progress = startProgress;
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
		if (sprites.Count < 1) return;
		if (!IsPlaying) return;

		Recalculate();

		if (playSelf) SelfPlayUpdate();

		foreach (var animEvent in spriteAnimEvents)
			animEvent.ProcessProgress(progress);
	}

	bool IsPlaying => loop || _plays < 1;

	/// <summary>
	/// Sets the progress and recalculates to show the correct frame
	/// </summary>
	public void SetProgress(float newProgress)
	{
		progress = newProgress;
		Recalculate();
	}

	void Recalculate()
	{
		int spriteIndex = Mathf.FloorToInt(progress * sprites.Count);
        if (spriteIndex >= sprites.Count) 
            spriteIndex = sprites.Count - 1;

		if (spriteRenderer)
        	spriteRenderer.sprite = sprites[spriteIndex];

		if (image)
			image.sprite = sprites[spriteIndex];
	}

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

	void OnEnable()
	{
		if (resetOnEnable)
			Reset();
	}

	[ButtonGroup]
	public void Reset() 
	{  
		_plays = 0;
		progress = 0;
	}

	[ButtonGroup]
	void InvertAnimation()
	{
		sprites.Reverse();
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