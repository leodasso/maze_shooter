using UnityEngine;
using Sirenix.OdinInspector;

public class FlipToVelocity : SpriteFlipper
{
    public new Rigidbody rigidbody;
    [MinValue(0)]
    public float minFlipVelocity = .05f;
    
    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(rigidbody.velocity.x) >= minFlipVelocity)
            UpdateScale(rigidbody.velocity.x);
    }
}
