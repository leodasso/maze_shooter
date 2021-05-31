using UnityEngine;
using UnityEngine.Events;
using Arachnid;
using Sirenix.OdinInspector;
using ShootyGhost;
using System.Collections.Generic;

public class MovementBase : MonoBehaviour, IControllable
{	
	[OnValueChanged("ApplyDynamicsProfile")]
	public DynamicsProfile movementProfile;

	[Tooltip("Multiplies the max speed")]
    public float speedMultiplier = 1;

	[Range(0, 1), Tooltip("If movement input is less than this, drag kicks in.")]
	public float moveInputThreshold = .25f;

    [Tooltip("Use an animation curve to control the speed over time? This is a multiplier.")]
    public bool useSpeedCurve;
    
    [ShowIf("useSpeedCurve")]
    public AnimationCurve speedCurve;

	[PropertyOrder(900), Title("Events")]
	public UnityEvent onGrounded;

	[PropertyOrder(900)]
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

	protected Vector3 totalVelocity;

    float _speedCurveTime;

	[ShowInInspector, ReadOnly]
	bool _isGrounded;

	[ShowInInspector, ReadOnly]
	HashSet<MovementMod> mods = new HashSet<MovementMod>();

	public void AddMod(MovementMod mod) 
	{
		mods.Add(mod);
	}

	public void RemoveMod(MovementMod mod)
	{
		mods.Remove(mod);
	}

    protected float TotalSpeedMultiplier()
    {
        if (!useSpeedCurve) return speedMultiplier;
        return speedMultiplier * speedCurve.Evaluate(_speedCurveTime);
    }

	protected virtual float TotalMaxSpeed => TotalSpeedMultiplier() * movementProfile.maxSpeed;
	protected virtual float TotalAcceleration => movementProfile.acceleration;
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        direction = Vector3.zero;
		ApplyDynamicsProfile();
    }

	protected virtual void ApplyDynamicsProfile() {
		if (!_rigidbody || !movementProfile) return;
		_rigidbody.useGravity = movementProfile.rigidbodyGravity;
	}

    protected virtual void Update()
    {
        if (useSpeedCurve)
        {
            _speedCurveTime += Time.deltaTime;
            if (_speedCurveTime > speedCurve.Duration())
                _speedCurveTime = 0;
        }
    }

	protected virtual void FixedUpdate() 
	{
		CalculateTotalVelocity();

		// if a move is desired, apply move velocity
		if (direction.magnitude > moveInputThreshold)
			_rigidbody.velocity = new Vector3(totalVelocity.x, _rigidbody.velocity.y, totalVelocity.z);

		// otherwise drag to a halt
		else {
			Vector3 zeroSpeed = new Vector3(0, _rigidbody.velocity.y, 0);
			totalVelocity = Vector3.Lerp(totalVelocity, Vector3.zero, Time.fixedDeltaTime * movementProfile.drag);
			_rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity, zeroSpeed, Time.fixedDeltaTime * movementProfile.drag);
		}
	}

	protected virtual void CalculateTotalVelocity() 
	{
		totalVelocity += direction * TotalAcceleration * Time.fixedDeltaTime;
		totalVelocity = Vector3.ClampMagnitude(totalVelocity, TotalMaxSpeed * direction.magnitude);

		foreach (var mod in mods)
			totalVelocity = mod.ModifyVelocity(totalVelocity);
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

	public Vector3 GetLastDirection()
	{
		return lastDirection;
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

    public virtual void DoActionAlpha() 
	{
		foreach (var mod in mods)
			mod.DoActionAlpha();
	}

	public void OnPlayerControlEnabled(bool isEnabled)
	{
		if (!isEnabled) {
			direction = Vector3.zero;
			lastDirection = Vector3.zero;
		}
	}

    public string Name()
    {
        return GetType().ToString() + " " + name;
    }

	public void RestartSpeedCurve() 
	{
		_speedCurveTime = 0;
	}
}
