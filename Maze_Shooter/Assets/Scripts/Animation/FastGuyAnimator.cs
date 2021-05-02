using ShootyGhost;
using UnityEngine;

[AddComponentMenu("Animation/Fast Guy Animator")]
public class FastGuyAnimator : CreatureAnimator
{
	public SpriteAnimation flying;

	bool _isFlying;

	public void BeginFlying() {_isFlying = true;}
	public void EndFlying() 
	{
		_isFlying = false;
		SetAnimImmediate(idle);
	}


	protected override void Update()
	{
		base.Update();
		if (_isFlying) SetAnim(flying);
	}
}
