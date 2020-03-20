using UnityEngine;

public class TrackBugAnimator : MonoBehaviour
{
    public SpriteAnimationPlayer animationPlayer;
    public PseudoVelocity pseudoVelocity;

    public SpriteAnimation idleAnimation;
    public SpriteAnimation walkingAnimation;
    public float idleSpeed = .1f;

    // Update is called once per frame
    void Update()
    {
        animationPlayer.spriteAnimation =
            pseudoVelocity.velocity.magnitude > idleSpeed ? walkingAnimation : idleAnimation;
    }
}