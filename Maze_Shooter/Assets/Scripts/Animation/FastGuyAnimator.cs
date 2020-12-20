using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShootyGhost;

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
