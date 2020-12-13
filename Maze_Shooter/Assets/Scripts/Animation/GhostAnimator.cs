using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShootyGhost;

public class GhostAnimator : MonoBehaviour
{
	public SpriteAnimationPlayer animationPlayer;
	public new Rigidbody rigidbody;
	public Haunter haunter;
	public SpriteAnimation idle;
	public SpriteAnimation run;
	public SpriteAnimation haunt;
	public SpriteAnimation appear;
	public float idleSpeed = .25f;

    // Update is called once per frame
    void Update()
    {
		if (haunter.ghostState == GhostState.Normal) 
		{
        animationPlayer.spriteAnimation =
            rigidbody.velocity.magnitude > idleSpeed ? run : idle;
		}
    }

	public void PlayHauntAnimation() 
	{
		animationPlayer.spriteAnimation = haunt;
		animationPlayer.PlayClipFromBeginning();
	}
}