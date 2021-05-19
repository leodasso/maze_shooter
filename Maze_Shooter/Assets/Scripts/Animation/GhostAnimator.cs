using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShootyGhost;
using Sirenix.OdinInspector;

[AddComponentMenu("Animation/Ghost Animator")]
public class GhostAnimator : CreatureAnimator
{
	public SpriteAnimation appear;

	[Space]
	public SpriteRenderer spriteRenderer;
	public new Rigidbody rigidbody;

	[ButtonGroup]
	public void Hide() 
	{
		spriteRenderer.enabled = false;
	}

	[ButtonGroup]
	public void Appear() 
	{
		OverrideAnimImmediate(appear);
		spriteRenderer.enabled = true;
	}
}