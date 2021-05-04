using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[ TypeInfoBox("Moves the playhead position of a sprite animation based on player input.") ]
public class SpriteAnimationMovement : MonoBehaviour, IControllable
{
	public float maxSpeed = 2;
	public float acceleration = 1;
	public float drag = 1;

	float currentDrag;
	float currentSpeed;

	[Tooltip("The axis along which to accept input. For example, if this is (0, 1), then vertical joystick input " +
	"will apply movement, but horizontal will not.")]
	public Vector2 inputAxis = Vector2.up;

	[Space, Tooltip("The sprite animator to move the progress of")]
	public SpriteAnimator spriteAnimator;

	void Start() 
	{
		if (spriteAnimator.playSelf) {
			Debug.LogError("Sprite animator linked to " + name + " is set to play self, so it can't be controlled by player input.", gameObject);
			enabled = false;
		}
	}

	void Update() 
	{
		currentSpeed = Mathf.Lerp(currentSpeed, 0, Time.deltaTime * currentDrag);

		spriteAnimator.progress += currentSpeed * Time.deltaTime;
		spriteAnimator.progress = Mathf.Clamp01(spriteAnimator.progress);
	}

	public void ApplyLeftStickInput(Vector2 input) 
	{
		float inputAmount = Vector2.Dot(input, inputAxis);

		currentSpeed += inputAmount * acceleration * Time.deltaTime;
		currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);
		
		// only have drag if there's no player input
		currentDrag = Mathf.Abs(inputAmount) < .25f ? drag : 0;
	}

	public void ApplyRightStickInput(Vector2 input) 
	{
	}

	public void DoActionAlpha() 
	{
	}

	public string Name() 
	{
		return "Sprite Animation Movement " + name;
	}
}
