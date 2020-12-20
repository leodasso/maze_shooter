using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShootyGhost;
using Sirenix.OdinInspector;

public class CreatureAnimator : MonoBehaviour
{
	public float idleSpeed = .5f;

	[Tooltip("Optional - used to determine wheather this is grounded")]
	public SpriteAnimationPlayer animationPlayer;

	[InlineProperty]
	public MovementSource velocitySource;
	
	public SpriteAnimation idle;
	public SpriteAnimation run;

    // Start is called before the first frame update
    void Start()
    {    }

    // Update is called once per frame
    void Update()
    {
		if (velocitySource.GetMovementVector().magnitude > idleSpeed) {
			animationPlayer.spriteAnimation = run;
		}
		else animationPlayer.spriteAnimation = idle;
    }
}
