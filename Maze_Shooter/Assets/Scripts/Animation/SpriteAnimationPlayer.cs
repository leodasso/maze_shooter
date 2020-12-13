 using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

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

	float TotalFrameRate => frameRate * speedMultiplier;
	float FrameDuration => 1 / TotalFrameRate;

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

		SetFacingVector(direction.GetMovementVector());

		if (speedChangesFramerate) 
			speedMultiplier = speedToFramerate.Evaluate(speedSource.GetMovementVector().magnitude);

		// determine the sprite list to use based on direction
		_frameProgress += Time.deltaTime;
		if (_frameProgress >= FrameDuration)
		{
			_frameProgress = 0;
			NextFrame();
		}
	}

	public void PlayClipFromBeginning() 
	{
		_currentFrame = 0;
		_frameProgress = 0;
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
		if (_currentFrame >= _currentAnimClip.Count)
			_currentFrame = 0;

		spriteRenderer.sprite = _currentAnimClip[_currentFrame];
	}
}