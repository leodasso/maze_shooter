using UnityEngine;

public class ConstantForce : MonoBehaviour
{
    public Vector3 force;
    Rigidbody _rigidbody;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        _rigidbody.AddForce(force * Time.fixedDeltaTime);
    }
}
