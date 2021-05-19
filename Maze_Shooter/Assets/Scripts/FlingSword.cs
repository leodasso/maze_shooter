using UnityEngine;
using Sirenix.OdinInspector;

public class FlingSword : MonoBehaviour
{
	[Tooltip("Uses rubber band to drive the flinging direction and events of the sword")]
	public RubberBand rubberBand;

	public new Rigidbody rigidbody;

	public SpriteRenderer aimIndicator;

	public float maxAim = 10;

	public AnimationCurve aimIndicatorAlpha = AnimationCurve.Linear(0, 0, 1, 1);

	[Space]
	public float flingSpeed = 10;
	[Tooltip("When it hits a terrain or other object, how high it bounces")]
	public float bounceHeight = 10;
	[Range(0, 1), Tooltip("Horizontal bounciness when hitting an object")]
	public float bounciness = .25f;

	[Space, Tooltip("Direction sword will go when Fling() is invoked")]
	public Vector3 flingDirection;

	public void Fling() 
	{
		Vector3 flingVel = flingDirection.normalized * flingSpeed;
		Debug.Log("Flinging at " + flingVel);
		rigidbody.velocity = flingVel;
	}

    // Update is called once per frame
    void Update()
    {
		if (rubberBand)
        	flingDirection = rubberBand.forceVector;

		if (aimIndicator) {
			aimIndicator.transform.localPosition = flingDirection.normalized * maxAim;
			float normalizedAimPower = rubberBand.forceVector.magnitude;
			aimIndicator.color = new Color(aimIndicator.color.r, aimIndicator.color.g, aimIndicator.color.b, aimIndicatorAlpha.Evaluate(normalizedAimPower));
		}
    }

	public void Bounce() 
	{
		Vector3 bounceVel = rigidbody.velocity * -bounciness;
		bounceVel = new Vector3(bounceVel.x, bounceHeight, bounceVel.z);
		rigidbody.velocity = bounceVel;
	}
}