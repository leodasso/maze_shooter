using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipToVelocity : SpriteFlipper
{
    public new Rigidbody rigidbody;
    
    // Update is called once per frame
    void Update()
    {
        UpdateScale(-rigidbody.velocity.x);
    }
}
