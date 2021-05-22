﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShootyGhost;
using Sirenix.OdinInspector;

[AddComponentMenu("Animation/Squire Animator")]
public class SquireAnimator : CreatureAnimator
{
	[Tooltip("When haunted, how hard player needs to pull before changing to 'pulling' anim")]
	[PropertyOrder(-100)]
	public float pullVectorTreshhold = .5f;

	public SpriteAnimation attackDash;
	public SpriteAnimation attack;
	public SpriteAnimation block;
	public SpriteAnimation panic;
	public SpriteAnimation pull;
	public SpriteAnimation haunted;
	public SpriteAnimation surprised;

	[Space]
	public RubberBand ghostOnSword;

	bool isHaunted;

	protected override void Update()
	{
		// we need to set a custom movement source for when 
		// the player is haunting the dude's sword.
		if (isHaunted) 
		{
			animationPlayer.direction.customDirection = ghostOnSword.forceVector;
			SpriteAnimation anim = ghostOnSword.forceVector.magnitude > pullVectorTreshhold ? pull : haunted;
			overrideAnim = anim;
			SetAnim(anim);
		}
		else base.Update();
	}

	public override void ClearOverride()
	{
		isHaunted = false;
		base.ClearOverride();
	}

	public void SetSurprised()
	{
		OverrideAnimImmediate(surprised);
	}
	

	public void SetPanic()
	{
		OverrideAnimImmediate(panic);
		ExitHaunted();
	}

	public void SetBlock()
	{
		OverrideAnimImmediate(block);
	}

	public void SetHaunted()
	{
		isHaunted = true;
	}

	public void ExitHaunted()
	{
		isHaunted = false;
	}

	public void SetAttack()
	{
		OverrideAnimImmediate(attack);
		ExitHaunted();
	}

	public void SetAttackDash()
	{
		OverrideAnimImmediate(attackDash);
	}
}
