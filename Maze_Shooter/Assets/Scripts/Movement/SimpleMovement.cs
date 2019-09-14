using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;

[RequireComponent(typeof(TargetFinder))]
public class SimpleMovement : MonoBehaviour
{
    public float speedMultiplier = 1;
    public FloatReference speed;
    TargetFinder _targetFinder;
    Rigidbody2D _rigidbody2D;
    
    // Start is called before the first frame update
    void Start()
    {
        _targetFinder = GetComponent<TargetFinder>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void FixedUpdate()
    {
        if (!_targetFinder) return;
        if (!_targetFinder.currentTarget) return;

        Vector2 dir = _targetFinder.currentTarget.position - transform.position;
        _rigidbody2D.AddForce(dir.normalized * speed.Value * speedMultiplier);
    }
}
