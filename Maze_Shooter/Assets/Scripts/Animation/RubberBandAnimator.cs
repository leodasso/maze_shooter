using System.Collections.Generic;
using UnityEngine;
using ShootyGhost;

public class RubberBandAnimator : MonoBehaviour
{
	public RubberBand rubberBand;
	public SpriteAnimationPlayer spriteAnimationPlayer;

	[Space]
	[Tooltip("picks an animation from start of list to end based on how stretched the rubber band is.")]
	public List<SpriteAnimation> animations = new List<SpriteAnimation>();

    // Start is called before the first frame update
    void Start()
    {    }

    // Update is called once per frame
    void Update()
    {
        int animIndex = Mathf.FloorToInt(rubberBand.NormalizedRadius * animations.Count);
		animIndex = Mathf.Clamp(animIndex, 0, animations.Count - 1);

		spriteAnimationPlayer.spriteAnimation = animations[animIndex];
    }
}