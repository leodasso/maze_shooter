using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShootyGhost;

[AddComponentMenu("Animation/Fling Sword Animator")]
public class FlingSwordAnimator : CreatureAnimator
{
	public SpriteAnimation beingPulled;

	[Space]
	public RubberBand rubberBand;

	protected override void Start()
	{
		base.Start();
		SetAnim(idle);
	}

	protected override void Update()
	{
		// animate for sword being pulled
		if (rubberBand && rubberBand.NormalizedRadius > .1f) {
			SetAnim(beingPulled);
			animationPlayer.direction.source = DirectionSourceType.Custom;
			animationPlayer.direction.customDirection = rubberBand.FlingDirection;
		}
		else {
			animationPlayer.direction.source = DirectionSourceType.Rigidbody;
			base.Update();
		}
	}
}
