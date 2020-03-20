using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SpriteAnimationPlayer : MonoBehaviour
{
    public enum DirectionSourceType
    {
        RigidbodyVelocity,
        MoverDirection,
        PseudoVelocity,
    }
    
    public float speedMultiplier = 1;
    public float frameRate = 12;
    public SpriteRenderer spriteRenderer;
    public SpriteAnimation spriteAnimation;

    [Space]
    public DirectionSourceType facingDirectionSource = DirectionSourceType.RigidbodyVelocity;
    
    [ShowIf("SourceIsRigidbody")]
    public new Rigidbody rigidbody;
    
    [ShowIf("SourceIsMover")]
    public MovementBase mover;

    [ShowIf("SourceIsPseudo")]
    public PseudoVelocity pseudoVelocity;
    float _frameProgress;
    int _currentFrame = 0;
    List<Sprite> _currentAnimClip = new List<Sprite>();
    Vector3 _forward = Vector3.right;

    float TotalFrameRate => frameRate * speedMultiplier;
    float FrameDuration => 1 / TotalFrameRate;
    
    // These are so odin knows which properties to show
    bool SourceIsRigidbody => facingDirectionSource == DirectionSourceType.RigidbodyVelocity;
    bool SourceIsMover => facingDirectionSource == DirectionSourceType.MoverDirection;
    bool SourceIsPseudo => facingDirectionSource == DirectionSourceType.PseudoVelocity;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position, _forward * 3);
    }

    // Start is called before the first frame update
    void Start()
    {
        NextFrame();
    }

    // Update is called once per frame
    void Update()
    {
        if (!spriteAnimation) return;

        switch (facingDirectionSource)
        {
                case DirectionSourceType.MoverDirection: SetFacingVector(mover.GetDirection());
                    break;
                
                case DirectionSourceType.RigidbodyVelocity: SetFacingVector(rigidbody.velocity);
                    break;
                
                case DirectionSourceType.PseudoVelocity: SetFacingVector(pseudoVelocity.velocity);
                    break;
        }
        
        // determine the sprite list to use based on direction

        _frameProgress += Time.deltaTime;
        if (_frameProgress >= FrameDuration)
        {
            _frameProgress = 0;
            NextFrame();
        }
    }
    
    void SetFacingVector(Vector3 forward)
    {
        if (forward.magnitude < .1f) return;
        _forward = new Vector3(forward.x, 0, forward.z);
        
        // horizontal flipping
        float dot = Vector3.Dot(_forward, Vector3.right);
        spriteRenderer.flipX = dot < 0;
    }


    void NextFrame()
    {
        _currentAnimClip = spriteAnimation.ClipForDirection(new Vector2(_forward.x, _forward.z));
        _currentFrame++;
        if (_currentFrame >= _currentAnimClip.Count)
            _currentFrame = 0;

        spriteRenderer.sprite = _currentAnimClip[_currentFrame];
    }
}