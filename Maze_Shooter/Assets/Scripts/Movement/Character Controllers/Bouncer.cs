using Arachnid;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using ShootyGhost;

public class Bouncer : MonoBehaviour
{
	public enum BounceType {
		PercentageOfVelocity,
		SetVelocity,
	}
	[ToggleLeft, SerializeField]
	bool debug;

	[ToggleLeft, Tooltip( "Automatically invoke bounce when colliding with something of the right layer & tags. Will use the opposite of rigidbody direction to calculate bounce direction."
	 + " \n if you want bounces to be tightly controlled from another script, disable autoBounce and call Bounce() or Bounce(Vector3)")]
	public bool autoBounce = true;

	[ToggleLeft, Tooltip("Freezes Y position when not bouncing, and frees it when bouncing.")]
	public bool controlRigidbodyConstraints = true;

	[InlineProperty]
	public MovementSource bounceDirectionSource;

	public BounceType horizontalBounceType = BounceType.SetVelocity;

	[MinMaxSlider(0, 999, true), Tooltip("Horizontal bounce speed will be a random value between these two numbers."), HideIf("isPercentageType")]
	/// note that X and Y here are min and max, not axes in 3d space
	public Vector2 horizontalSpeed = new Vector2(10, 12);

	[Range(0,1), ShowIf("isPercentageType"), Tooltip("Amount of horizontal velocity to keep when bouncing")]
	public float horizontalBouncePercent = .75f;

	public float bounceHeightSpeed = 15;
    public new Rigidbody rigidbody;

    [SerializeField, Tooltip("Only regocnize collisions in these layers"), FormerlySerializedAs("controlledBounceImpacts")]
    LayerMask layerMask;

	[SerializeField, Tooltip("If collides with objects with these tags, won't bounce.")]
	public List<string> omitBounceTags = new List<string>();

	[SerializeField, Tooltip("Invoked when a collision that will cause a bounce happens")]
	UnityEvent onBounceCollision;

	[SerializeField]
    UnityEvent onBounceBegin;

	RigidbodyConstraints initConstraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
	RigidbodyConstraints bounceConstraints = RigidbodyConstraints.FreezeRotation;

	bool isPercentageType => horizontalBounceType == BounceType.PercentageOfVelocity;

	void Start()
	{
		if (controlRigidbodyConstraints)
			rigidbody.constraints = initConstraints;
	}

    void OnCollisionEnter(Collision other)
    {
		if (!enabled) return;
		if (omitBounceTags.Contains(other.gameObject.tag)) return;
		if (!Math.LayerMaskContainsLayer(layerMask, other.gameObject.layer)) return;

		if (debug) 
			Debug.Log(name + " colliding with " + other.gameObject.name + " in layer " + LayerMask.LayerToName(other.gameObject.layer));

		onBounceCollision.Invoke();

		if (autoBounce) {
			// rigidbody velocity might not be correct, so get the velocity from the collision info
			if (bounceDirectionSource.source == DirectionSourceType.Rigidbody)
				Bounce(other.relativeVelocity);

			else
				Bounce(-bounceDirectionSource.GetMovementVector());
		}
    }

	Vector3 BounceVelocity(Vector3 input)
	{
		if (isPercentageType)
			return new Vector3(input.x * horizontalBouncePercent, bounceHeightSpeed, input.z * horizontalBouncePercent);

		else
			return input.normalized * Random.Range(horizontalSpeed.x, horizontalSpeed.y) + Vector3.up * bounceHeightSpeed;
	}

	public void Bounce(Vector3 dir) 
	{
		// dir.Normalize();
		Debug.Log(name + " given a bounce with " + dir);
		BounceInternal(BounceVelocity(dir));
	}

    public void Bounce()
    {
        float yVelocity = bounceHeightSpeed;
		BounceInternal(BounceVelocity(HorizontalBounceSpeed));
    }

	Vector3 HorizontalBounceSpeed => new Vector3(
                Random.Range(horizontalSpeed.x, horizontalSpeed.y), // X
                0,													// Y
                Random.Range(horizontalSpeed.x, horizontalSpeed.y)	// Z
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