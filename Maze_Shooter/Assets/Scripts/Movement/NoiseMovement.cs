using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using Random = UnityEngine.Random;

public class NoiseMovement : MovementBase
{
    public float minDecisionTime = .25f;
    public float maxDecisionTime = 1;

    protected override void Start()
    {
        base.Start();
        ChooseDirection();
    }


    void ChooseDirection()
    {
        direction = Random.onUnitSphere;
        float waitTime = Random.Range(minDecisionTime, maxDecisionTime);
        Invoke(nameof(ChooseDirection), waitTime);
    }

    void FixedUpdate()
    {
        Vector3 flatDirection = new Vector3(direction.x, 0, direction.z);
        _rigidbody.AddForce(flatDirection * TotalSpeedMultiplier() * movementProfile.movementForce);
    }
}
