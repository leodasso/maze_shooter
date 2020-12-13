using UnityEngine;
using Arachnid;
using Sirenix.OdinInspector;
using ShootyGhost;

public class MovementBase : MonoBehaviour, IControllable
{
	[OnValueChanged("ApplyDynamicsProfile")]
	public DynamicsProfile movementProfile;
    public float speedMultiplier = 1;

    [Tooltip("Use an animation curve to control the speed over time? This is a multiplier.")]
    public bool useSpeedCurve;
    
    [ShowIf("useSpeedCurve")]
    public AnimationCurve speedCurve;
    
    [Tooltip("Optional - will set the current path tangent to the sprite animation player")]
    public SpriteAnimationPlayer spriteAnimationPlayer;
    
    protected Rigidbody _rigidbody;
    
    /// <summary>
    /// The intended direction of movement. Differs from velocity in that if there's a wall or blocker and
    /// velocity is 0, this will still show the intended direction.
    /// </summary>
    protected Vector3 direction;

    float _speedCurveTime;

    protected float TotalSpeedMultiplier()
    {
        if (!useSpeedCurve) return speedMultiplier;
        return speedMultiplier * speedCurve.Evaluate(_speedCurveTime);
    }
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        direction = Vector3.zero;
		ApplyDynamicsProfile();
    }

	protected virtual void ApplyDynamicsProfile() {
		if (!_rigidbody) return;
		_rigidbody.mass = movementProfile.rigidbodyMass;
		_rigidbody.drag = movementProfile.rigidbodyDrag;
		_rigidbody.useGravity = movementProfile.rigidbodyGravity;
	}

    protected virtual void Update()
    {
        if (useSpeedCurve)
        {
            _speedCurveTime += Time.deltaTime;
            if (_speedCurveTime > speedCurve.Duration())
            {
                _speedCurveTime = 0;
            }
        }
    }

    public Vector3 GetDirection()
    {
        return direction;
    }


    public void IncreaseSpeedMultiplier(float amt)
    {
        speedMultiplier += amt;
    }

    public void SetSpeedMultiplier(float value)
    {
        speedMultiplier = value;
    }
    
    public virtual void ApplyLeftStickInput(Vector2 input)
    {
        direction = Math.Project2Dto3D(Vector2.ClampMagnitude(input, 1));
    }

    public virtual void ApplyRightStickInput(Vector2 input) { }

    public virtual void DoActionAlpha() { }

    public string Name()
    {
        return GetType().ToString() + " " + name;
    }
}
