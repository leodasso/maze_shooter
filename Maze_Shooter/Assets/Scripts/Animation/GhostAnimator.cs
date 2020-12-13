using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShootyGhost;
using Sirenix.OdinInspector;

public class GhostAnimator : MonoBehaviour
{
	public SpriteRenderer spriteRenderer;
	public SpriteAnimationPlayer animationPlayer;
	public new Rigidbody rigidbody;
	public Haunter haunter;
	public SpriteAnimation idle;
	public SpriteAnimation run;
	public SpriteAnimation appear;
	public float idleSpeed = .25f;

	bool _playingAppearAnim;

	[ButtonGroup]
	public void Hide() 
	{
		spriteRenderer.enabled = false;
	}

	[ButtonGroup]
	public void Appear() 
	{
		_playingAppearAnim = true;
		animationPlayer.PlayClipFromBeginning(appear, FinishAppearAnimation);
		spriteRenderer.enabled = true;
	}

	void FinishAppearAnimation() 
	{
		_playingAppearAnim = false;
	}


    // Update is called once per frame
    void Update()
    {
		if (haunter.ghostState == GhostState.Normal && !_playingAppearAnim) 
		{
			animationPlayer.spriteAnimation =
				rigidbody.velocity.magnitude > idleSpeed ? run : idle;
		}
    }
}