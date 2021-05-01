using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[ TypeInfoBox("Moves the playhead position of a sprite animation based on player input.") ]
public class SpriteAnimationMovement : MonoBehaviour, IControllable
{
	public float speed = 1;

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

	public void ApplyLeftStickInput(Vector2 input) 
	{
		float inputAmount = Vector2.Dot(input, inputAxis);
		spriteAnimator.progress += inputAmount * speed * Time.deltaTime;
		spriteAnimator.progress = Mathf.Clamp01(spriteAnimator.progress);
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
