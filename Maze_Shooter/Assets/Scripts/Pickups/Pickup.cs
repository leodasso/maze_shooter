using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class Pickup : MonoBehaviour
{
	[ToggleLeft]
	public bool jumpOnStart;

	[FormerlySerializedAs("spawnVelocity")]
	public Vector3 jumpVelocity = new Vector3(0, 5, 0);

	[FormerlySerializedAs("spawnVelocityRandomness")]
    public float jumpVelocityRandomness = 3;

	[SerializeField, Tooltip("Delay in seconds between getting gulped and destroying"), MinValue(0)]
	float destroyDelay = .5f;

	[SerializeField]
    protected UnityEvent onGrabbed;


	[SerializeField]
	protected new Rigidbody rigidbody;
 
    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (jumpOnStart) Jump();
    }

	[Button]
	public void Jump() 
	{
		rigidbody.isKinematic = false;
        rigidbody.velocity = jumpVelocity + Random.insideUnitSphere * jumpVelocityRandomness;
	}

	public void GetGulped()
    {
        onGrabbed.Invoke();
		var col = GetComponent<Collider>();
		if (col)
			col.enabled = false;
        Destroy(gameObject, destroyDelay);
    }
}
