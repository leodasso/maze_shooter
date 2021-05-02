using ShootyGhost;
using UnityEngine;

[AddComponentMenu("Animation/Teardrop Animator")]
public class TeardropAnimator : CreatureAnimator
{
	public SpriteAnimation spawnAnim;

	public void SetSpawn() 
	{
		overrideAnim = spawnAnim;
		SetAnimImmediate(spawnAnim);
	}

	protected override void Update()
	{
		if (!overrideAnim)
			base.Update();
	}
}