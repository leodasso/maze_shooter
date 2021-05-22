using UnityEngine;
using Arachnid;
using Sirenix.OdinInspector;

[TypeInfoBox("Give the animator info based on the player input and rigidbody velocity. To set up, add float parameters 'speed' " +
             "and 'facingAngle' to the Animator you want to control.")]
public class MovementAnimation : MonoBehaviour, IControllable
{
    public enum VelocitySource
    {
        Rigidbody,
        PseudoVelocity,
    }
    
    [Tooltip("(optional) Animator to send speed and movement angle to")]
    public Animator animator;

	public SpriteAnimator spriteAnimator;

    [ToggleGroup("setSpeed")]
    public bool setSpeed;
    
    [ToggleGroup("setSpeed")]
    [Tooltip("Choose the source to get the velocity of this object. This is used to set the 'speed' parameter of the animator.")]
    public VelocitySource velocitySource;
    
    [ToggleGroup("setSpeed"), ShowIf("UsesRigidbody")]
    public new Rigidbody rigidbody;
    
    [ToggleGroup("setSpeed"), HideIf("UsesRigidbody")]
    public PseudoVelocity pseudoVelocity;

	[ToggleGroup("setSpeed")]
	public AnimationCurve speedToFramerate = AnimationCurve.Linear(0, 0, 10, 12);

    [ToggleGroup("setMoveAngle")] 
    public bool setMoveAngle;
    
    [Tooltip("This doesn't affect movement! Just offsets the movement angle that gets sent to the animator.")]
    [ToggleGroup("setMoveAngle")] 
    public float movementAngleOffset = 45;

    bool UsesRigidbody => velocitySource == VelocitySource.Rigidbody;

    float _velocity;
    
    
    void Update()
    {
        if (setSpeed) SetSpeedUpdate();
    }

	void SetSpeedUpdate()
	{
		if (UsesRigidbody && !rigidbody) return;
		if (!UsesRigidbody && !pseudoVelocity) return;

		_velocity = UsesRigidbody ? rigidbody.velocity.magnitude : pseudoVelocity.velocity.magnitude;

		if (spriteAnimator) spriteAnimator.frameRate = speedToFramerate.Evaluate(_velocity);
		if (animator) animator.SetFloat("speed", _velocity);
	}

    public void ApplyLeftStickInput(Vector2 input)
    {
        if (!setMoveAngle) return;
        if (!animator) return;
        animator.SetFloat("facingAngle", Math.AngleFromVector2(input, movementAngleOffset));
    }

    public void ApplyRightStickInput(Vector2 input)
    {}

	public void OnPlayerControlEnabled(bool isEnabled)	{}

    public void DoActionAlpha() {}

    public string Name()
    {
        return "MovementAnimation" + name;
    }
}
