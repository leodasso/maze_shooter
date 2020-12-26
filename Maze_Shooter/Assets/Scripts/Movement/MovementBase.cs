using UnityEngine;
using UnityEngine.Events;
using Arachnid;
using Sirenix.OdinInspector;
using ShootyGhost;

public class MovementBase : MonoBehaviour, IControllable
{
	public static float gravity = -25;
	
	[OnValueChanged("ApplyDynamicsProfile")]
	public DynamicsProfile movementProfile;
    public float speedMultiplier = 1;

    [Tooltip("Use an animation curve to control the speed over time? This is a multiplier.")]
    public bool useSpeedCurve;
    
    [ShowIf("useSpeedCurve")]
    public AnimationCurve speedCurve;

	public Arena arena;
    

	[PropertyOrder(900), FoldoutGroup("events")]
	public UnityEvent onGrounded;

	[PropertyOrder(900), FoldoutGroup("events")]
	public UnityEvent onUnGrounded;
    
    protected Rigidbody _rigidbody;

	public bool IsGrounded => _isGrounded;
    
    /// <summary>
    /// The intended direction of movement. Differs from velocity in that if there's a wall or blocker and
    /// velocity is 0, this will still show the intended direction.
    /// </summary>
    protected Vector3 direction;

	/// <summary>
	/// Usually same as direction, but will remember the player's last direction if they stop input.
	/// </summary>
	protected Vector3 lastDirection;

    float _speedCurveTime;

	[ShowInInspector, ReadOnly]
	bool _isGrounded;

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
		if (!_rigidbody || !movementProfile) return;
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

	protected virtual void OnCollisionEnter(Collision other) 
	{
		if (other.transform.tag == "Ground") {
			if (!_isGrounded) {
				onGrounded.Invoke();
				_isGrounded = true;
			}
		}
	}

		protected virtual void OnCollisionExit(Collision other) 
	{
		if (other.transform.tag == "Ground") {
			if (_isGrounded) {
				onUnGrounded.Invoke();
				_isGrounded = false;
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
		if (!enabled) return;
        direction = Math.Project2Dto3D(Vector2.ClampMagnitude(input, 1));
		if (direction.magnitude > .3f) lastDirection = direction;
		lastDirection.Normalize();
    }

    public virtual void ApplyRightStickInput(Vector2 input) { }

    public virtual void DoActionAlpha() { }

    public string Name()
    {
        return GetType().ToString() + " " + name;
    }

	public void ChooseDirection()
    {
		if (arena) {
			Vector3 newPoint = arena.GetPoint();
			direction = (newPoint - transform.position).normalized;
		}
        else
			direction = Random.onUnitSphere;
    }
}
