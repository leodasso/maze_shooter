using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShootyGhost;

public class GhostAnimator : MonoBehaviour
{
	public SpriteAnimationPlayer animationPlayer;
	public new Rigidbody rigidbody;
	public Haunter haunter;

	public AnimationCurve speedToFramerate = AnimationCurve.EaseInOut(0, 0, 1, 1);

	public SpriteAnimation idle;
	public SpriteAnimation run;
	public SpriteAnimation haunt;
	public float idleSpeed = .25f;


    // Update is called once per frame
    void Update()
    {
		animationPlayer.speedMultiplier = speedToFramerate.Evaluate(rigidbody.velocity.magnitude);

        animationPlayer.spriteAnimation =
            rigidbody.velocity.magnitude > idleSpeed ? run : idle;

		if (haunter.ghostState == GhostState.Targeting) 
			animationPlayer.spriteAnimation = haunt;
    }
}
