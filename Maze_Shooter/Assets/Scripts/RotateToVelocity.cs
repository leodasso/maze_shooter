using UnityEngine;
using Arachnid;
using Sirenix.OdinInspector;

[TypeInfoBox("Select a rigidbody2D, and this will rotate this object based on the velocity of the rigidbody.")]
public class RotateToVelocity : MonoBehaviour
{
    public new Rigidbody2D rigidbody2D;
    [Range(-360, 360)]
    public float angleOffset;
    [Tooltip("The velocity magnitude must be above this for rotation to occur")]
    public float minVelocity = 1;

    [ReadOnly, ShowInInspector]
    Vector2 _velocity;

    [ReadOnly, ShowInInspector]
    float _velocityMagnitude;

    // Update is called once per frame
    void Update()
    {
        if (!rigidbody2D) return;
        _velocity = rigidbody2D.velocity;
        _velocityMagnitude = _velocity.magnitude;
        if (_velocityMagnitude < minVelocity) return;
        float angle = Math.AngleFromVector2(rigidbody2D.velocity, angleOffset);
        
        transform.eulerAngles = new Vector3(0, 0, angle);
    }
}