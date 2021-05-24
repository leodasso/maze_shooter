using UnityEngine;
using UnityEngine.Events;

public class Pickup : MonoBehaviour
{
	public Vector3 spawnVelocity = new Vector3(0, 5, 0);
    public float spawnVelocityRandomness = 3;

	[SerializeField]
	float destroyDelay = .5f;

	[SerializeField]
    protected UnityEvent onGrabbed;
	public bool jumpOnStart;

	[SerializeField]
	protected new Rigidbody rigidbody;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (jumpOnStart) Jump();
    }

	public void Jump() 
	{
		rigidbody.isKinematic = false;
        rigidbody.velocity = spawnVelocity + Random.insideUnitSphere * spawnVelocityRandomness;
	}

	public void Grab()
    {
        onGrabbed.Invoke();
        Destroy(gameObject, destroyDelay);
    }
}
