using System.Collections;
using System.Collections.Generic;
using Arachnid;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class NoiseMovement : MovementBase
{
    [Tooltip("Profile for the stats of the perlin noise.")]
    public NoiseProfile noiseProfile;
    
    [Tooltip("If the group tends to bunch up in one corner or another after a while, use this to adjust their overall movement.")]
    public Vector2 noiseCalibration = new Vector2(-.5f, -.5f);

    [Tooltip("Noise is based on world position, so 2 noiseMovement things in a similar area will move in a similar way." +
             " If this is enabled, a random offset is generated on start. So two noise things will have different movement when" +
             " close together.")]
    public bool randomizeNoise;

    float _noiseScrollOffset = 0;
    Vector2 _noiseRandomOffset = Vector2.zero;

    protected override void Start()
    {
        base.Start();
        _noiseRandomOffset = Random.insideUnitCircle * 50;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 noisePos = transform.position + Math.Project2Dto3D(GeneratedPerlinVector());
        Gizmos.DrawSphere(noisePos, .05f);
        Gizmos.DrawLine(transform.position, noisePos);
    }
    
    void FixedUpdate()
    {
        _noiseScrollOffset += Time.fixedDeltaTime * noiseProfile.scrollSpeed.x;
        direction = Math.Project2Dto3D(GeneratedPerlinVector());
        _rigidbody.AddForce(direction * speed.Value * speedMultiplier);
    }
    
    
    Vector2 GeneratedPerlinVector()
    {
        float inputX = transform.position.x - _noiseScrollOffset;
        float inputY = transform.position.z + _noiseScrollOffset;

        if (randomizeNoise)
        {
            inputX += _noiseRandomOffset.x;
            inputY += _noiseRandomOffset.y;
        }

        float x = Math.CleanCenteredNoise(inputX, inputY, noiseProfile.frequency) + noiseCalibration.x;
        float y = Math.CleanCenteredNoise(inputX, inputY, noiseProfile.frequency) + noiseCalibration.y;
        return new Vector2(x, y);
    }


}
