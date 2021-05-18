using UnityEngine;
using Sirenix.OdinInspector;

public class FlingSword : MonoBehaviour
{
	[Tooltip("Uses rubber band to drive the flinging direction and events of the sword")]
	public RubberBand rubberBand;
	
	[Tooltip("Transform where ghost will return when haunting complete. This is moved by this component")]
	public Transform hauntReturnPos;

	public new Rigidbody rigidbody;

	[Tooltip("distance that the ghost will be returned to when unhaunting.")]
	public float ghostHauntReturnDist = 6;
	public float flingSpeed = 10;
	[Tooltip("When it hits a terrain or other object, how high it bounces")]
	public float bounceHeight = 10;
	[Range(0, 1), Tooltip("Horizontal bounciness when hitting an object")]
	public float bounciness = .25f;

	Vector3 flingDirection;

	public void Fling() 
	{
		rigidbody.velocity = flingDirection.normalized * flingSpeed;
	}

    // Update is called once per frame
    void Update()
    {
        flingDirection = rubberBand.FlingDirection;

		if (hauntReturnPos) {
			Vector3 hauntReturn = flingDirection * ghostHauntReturnDist;
			if (hauntReturn.magnitude < 3)
				hauntReturn = Vector3.right * 3;
			hauntReturnPos.position = transform.position + Vector3.Scale(hauntReturn, new Vector3(1, 0, 1));
		}
    }

	public void Bounce() 
	{
		Vector3 bounceVel = rigidbody.velocity * -bounciness;
		bounceVel = new Vector3(bounceVel.x, bounceHeight, bounceVel.z);
		rigidbody.velocity = bounceVel;
	}
}