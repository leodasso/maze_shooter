using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShootyGhost;

[AddComponentMenu("Animation/Squire Animator")]
public class SquireAnimator : CreatureAnimator
{
	public SpriteAnimation attack;
	public SpriteAnimation block;
	public SpriteAnimation panic;
	public SpriteAnimation pull;
	public SpriteAnimation spawn;
	public SpriteAnimation surprised;
}
