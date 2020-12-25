using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
}