using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace ShootyGhost {

	public enum DirectionSourceType
	{
		Rigidbody,
		Mover,
		PseudoVelocity,
		Custom,
	}

	public class SpriteAnimationPlayer : MonoBehaviour
	{
		public float speedMultiplier = 1;
		public float frameRate = 12;
		public bool realTime;
		public SpriteRenderer spriteRenderer;
		public SpriteAnimation spriteAnimation;

		[InlineProperty, Space]
		public MovementSource direction;

		[Space]
		public bool speedChangesFramerate;

		[ShowIf("speedChangesFramerate"), InlineProperty]
		public MovementSource speedSource;

		[ShowIf("speedChangesFramerate")]
		public AnimationCurve speedToFramerate = AnimationCurve.EaseInOut(0, 0, 1, 1);

		float _frameProgress;
		int _currentFrame = 0;
		List<Sprite> _currentAnimClip = new List<Sprite>();
		Vector3 _forward = Vector3.right;

		float TotalFrameRate => spriteAnimation.frameRateMultiplier * frameRate * speedMultiplier;
		float FrameDuration => 1 / TotalFrameRate;

		Action ClipFinished;

		SpriteAnimation prevFrameAnimation;

		float deltaTime => realTime ? Time.unscaledDeltaTime : Time.deltaTime;

		void OnDrawGizmos()
		{
			Gizmos.color = Color.magenta;
			Gizmos.DrawRay(transform.position, _forward * 3);
		}

		// Start is called before the first frame update
		void Start()
		{
			NextFrame();
		}

		// Update is called once per frame
		void Update()
		{
			if (!spriteAnimation) return;

			// Keep track of changes in sprite animation
			if (spriteAnimation != prevFrameAnimation) 
				OnClipChanged();
			prevFrameAnimation = spriteAnimation;

			SetFacingVector(direction.GetMovementVector());

			if (speedChangesFramerate) 
				speedMultiplier = speedToFramerate.Evaluate(speedSource.GetMovementVector().magnitude);

			// determine the sprite list to use based on direction
			_frameProgress += deltaTime;
			if (_frameProgress >= FrameDuration)
			{
				_frameProgress = 0;
				NextFrame();
			}
		}

		// The animation clip has been changed on this frame
		void OnClipChanged() 
		{
			if (!spriteAnimation) return;
			if (spriteAnimation.alwaysStartFromBeginning)
				PlayClipFromBeginning();
		}

		public void PlayClipFromBeginning() 
		{
			_currentFrame = 0;
			_frameProgress = 0;
			NextFrame();
		}

		public void PlayClipFromBeginning(Action onClipComplete) 
		{
			ClipFinished = onClipComplete;
			PlayClipFromBeginning();
		}

		public void PlayClipFromBeginning(SpriteAnimation clip, Action onClipComplete) 
		{
			ClipFinished = onClipComplete;
			spriteAnimation = clip;
			PlayClipFromBeginning();
		}

		void SetFacingVector(Vector3 forward)
		{
			if (forward.magnitude < .1f) return;
			_forward = new Vector3(forward.x, 0, forward.z);

			// horizontal flipping
			float dot = Vector3.Dot(_forward, Vector3.right);
			spriteRenderer.flipX = dot < 0;
		}


		void NextFrame()
		{
			_currentAnimClip = spriteAnimation.ClipForDirection(new Vector2(_forward.x, _forward.z));
			_currentFrame++;

			if (_currentFrame >= _currentAnimClip.Count) {

				// Invoke method when clip is done
				if (ClipFinished != null) {
					ClipFinished.Invoke();
					ClipFinished = null;
				}

				// handle looping / clamp
				if (spriteAnimation.playMode == PlayMode.Loop)
					_currentFrame = 0;
				else _currentFrame--;
			}

			spriteRenderer.sprite = _currentAnimClip[_currentFrame];
		}
	}
}