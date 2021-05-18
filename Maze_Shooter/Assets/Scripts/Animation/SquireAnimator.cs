using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShootyGhost;

[AddComponentMenu("Animation/Squire Animator")]
public class SquireAnimator : CreatureAnimator
{
	public SpriteAnimation attackDash;
	public SpriteAnimation attack;
	public SpriteAnimation block;
	public SpriteAnimation panic;
	public SpriteAnimation pull;
	public SpriteAnimation surprised;

	public void SetSurprised()
	{
		OverrideAnimImmediate(surprised);
	}

	public void SetPanic()
	{
		overrideAnim = panic;
		SetAnim(panic);
	}

	public void SetBlock()
	{
		OverrideAnimImmediate(block);
	}

	public void SetPull()
	{
		OverrideAnimImmediate(pull);
	}

	public void SetAttack()
	{
		OverrideAnimImmediate(attack);
	}

	public void SetAttackDash()
	{
		OverrideAnimImmediate(attackDash);
	}
}
