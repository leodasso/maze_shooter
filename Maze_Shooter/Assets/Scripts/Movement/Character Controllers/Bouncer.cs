using Arachnid;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using ShootyGhost;

public class Bouncer : MonoBehaviour
{
	[ToggleLeft, Tooltip( "Automatically invoke bounce when colliding with something of the right layer & tags. Will use the opposite of rigidbody direction to calculate bounce direction." + 
	" \n if you want bounces to be tightly controlled from another script, disable autoBounce and call Bounce() or Bounce(Vector3)")]
	public bool autoBounce = true;

	[ToggleLeft, Tooltip("Freezes Y position when not bouncing, and frees it when bouncing.")]
	public bool controlRigidbodyConstraints = true;

	[InlineProperty]
	public MovementSource bounceDirectionSource;

	[MinMaxSlider(0, 999, true), Tooltip("Horizontal bounce speed will be a random value between these two numbers.")]
	/// note that X and Y here are min and max, not axes in 3d space
	public Vector2 horizontalSpeed = new Vector2(10, 12);
	public float bounceHeightSpeed = 15;
    public new Rigidbody rigidbody;

    [SerializeField, Tooltip("Only collisions with objects of these layers will have a bounce controlled by this component")]
    LayerMask controlledBounceImpacts;

	[SerializeField, Tooltip("If collides with objects with these tags, won't bounce.")]
	public List<string> omitBounceTags = new List<string>();

	[SerializeField, Tooltip("Invoked when a collision that will cause a bounce happens")]
	UnityEvent onBounceCollision;

	[SerializeField]
    UnityEvent onBounceBegin;

	RigidbodyConstraints initConstraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
	RigidbodyConstraints bounceConstraints = RigidbodyConstraints.FreezeRotation;

	void Start()
	{
		if (controlRigidbodyConstraints)
			rigidbody.constraints = initConstraints;
	}

    void OnCollisionEnter(Collision other)
    {
		if (!enabled) return;
		if (omitBounceTags.Contains(other.gameObject.tag)) return;

		onBounceCollision.Invoke();

		if (!autoBounce) return;

        if (Math.LayerMaskContainsLayer(controlledBounceImpacts, other.gameObject.layer))
            Bounce(-bounceDirectionSource.GetMovementVector());
    }

	public void Bounce(Vector3 normalizedDirection) 
	{
		normalizedDirection.Normalize();
		BounceInternal(normalizedDirection * Random.Range(horizontalSpeed.x, horizontalSpeed.y) + Vector3.up * bounceHeightSpeed);
	}

    public void Bounce()
    {
        float yVelocity = bounceHeightSpeed;
		BounceInternal(Vector3.up * yVelocity + HorizontalBounceSpeed);
    }

	Vector3 HorizontalBounceSpeed => new Vector3(
                Random.Range(horizontalSpeed.x, horizontalSpeed.y), 
                0,
                Random.Range(horizontalSpeed.x, horizontalSpeed.y)
            );

	void BounceInternal(Vector3 velocity)
	{
		if (controlRigidbodyConstraints)
			rigidbody.constraints = bounceConstraints;

        rigidbody.velocity = velocity;
        onBounceBegin.Invoke();
	}

	public void ResetConstraints() {
		if (!controlRigidbodyConstraints) return;

		rigidbody.constraints = initConstraints;
	}
}