using ShootyGhost;
using UnityEngine;

[AddComponentMenu("Animation/Zappie Animator")]
public class ZappieAnimator : CreatureAnimator
{
	public SpriteAnimation chargingAnim;
	public SpriteAnimation attackAnim;
	public SpriteAnimation restAnim;

	public void SetCharging() {
		overrideAnim = chargingAnim;
	}

	public void SetAttack() {
		overrideAnim = attackAnim;
	}

	public void SetRest() {
		overrideAnim = restAnim;
	}

	protected override void Update()
	{
		base.Update();
		if (overrideAnim) SetAnim(overrideAnim);
	}
}
