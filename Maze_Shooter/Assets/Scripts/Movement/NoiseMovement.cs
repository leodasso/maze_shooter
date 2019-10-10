using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;

public class NoiseMovement : MovementBase
{    
    [Tooltip("The frequency or size of the noise. Larger frequency will result in smaller, jittery movements")]
    public FloatReference noiseFrequency;
    
    [Tooltip("The scroll speed of the perlin noise.")]
    public FloatReference scrollSpeed;
    
    [Tooltip("If the group tends to bunch up in one corner or another after a while, use this to adjust their overall movement.")]
    public Vector2 noiseCalibration = new Vector2(-.5f, -.5f);

    float _noiseScrollOffset = 0;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere((Vector2)transform.position + GeneratedPerlinVector(), .05f);
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + GeneratedPerlinVector());
    }
    
    void FixedUpdate()
    {
        _noiseScrollOffset += Time.fixedDeltaTime * scrollSpeed.Value;
        _direction = GeneratedPerlinVector();
        _rigidbody2D.AddForce(_direction * speed.Value * speedMultiplier);
    }
    
    
    Vector2 GeneratedPerlinVector()
    {
        float inputX = transform.position.x - _noiseScrollOffset;
        float inputY = transform.position.y + _noiseScrollOffset;
        float x = Mathf.PerlinNoise(inputX * noiseFrequency.Value, inputY * noiseFrequency.Value) + noiseCalibration.x;
        float y = Mathf.PerlinNoise(inputY * noiseFrequency.Value, inputX * noiseFrequency.Value) + noiseCalibration.y;
        return new Vector2(x, y);
    }


}
