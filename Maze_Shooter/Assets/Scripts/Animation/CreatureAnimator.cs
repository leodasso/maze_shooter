using UnityEngine;
using ShootyGhost;
using Sirenix.OdinInspector;

[AddComponentMenu("Animation/Creature Animator")]
public class CreatureAnimator : MonoBehaviour
{
	public float idleSpeed = .5f;

	public SpriteAnimationPlayer animationPlayer;

	[InlineProperty]
	public MovementSource velocitySource;

	[ReadOnly]
	public SpriteAnimation overrideAnim;

	[Space]
	public SpriteAnimation spawn;
	public SpriteAnimation idle;
	public SpriteAnimation run;

    // Start is called before the first frame update
    protected virtual void Start()
    {    }

    // Update is called once per frame
    protected virtual void Update()
    {
		if (overrideAnim) return;

		if (velocitySource.GetMovementVector().magnitude > idleSpeed) {
			SetAnim(run);
		}
		else SetAnim(idle);
    }

	public void SetSpawn()
	{
		overrideAnim = spawn;
		SetAnimImmediate(spawn);
	}

	public void ClearOverride() {
		overrideAnim = null;
	}

	protected void SetAnim(SpriteAnimation newAnim) {
		animationPlayer.spriteAnimation = newAnim;
	}

	protected void SetAnimImmediate(SpriteAnimation newAnim) {
		animationPlayer.spriteAnimation = newAnim;
		animationPlayer.PlayClipFromBeginning();
	}

	/// <summary>
	/// Overrides and immediately plays the given animation clip
	/// </summary>
	protected void OverrideAnimImmediate(SpriteAnimation newAnim) 
	{
		overrideAnim = newAnim;
		SetAnimImmediate(newAnim);
	}
}
