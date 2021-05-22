using ShootyGhost;
using UnityEngine;

[AddComponentMenu("Animation/Fast Guy Animator")]
public class FastGuyAnimator : CreatureAnimator
{
	public SpriteAnimation flying;

	public void BeginFlying() 
	{
		overrideAnim = flying;
		SetAnimImmediate(flying);
	}

	public void EndFlying() 
	{
		ClearOverride();
		SetAnimImmediate(idle);
	}
}
