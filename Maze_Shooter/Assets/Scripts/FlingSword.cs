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
}