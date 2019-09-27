using System.Collections;
using System.Collections.Generic;
using Arachnid;
using Sirenix.OdinInspector;
using UnityEngine;

public class SimpleMovement : MonoBehaviour
{
    public float speedMultiplier = 1;
    public FloatReference speed;
    
    [Tooltip("The thing I'll move towards. Keep in mind if there's a tergetFinder referenced, it will overwrite" +
             " whatever you put in here.")]
    public GameObject target;
    
    [Tooltip("(optional) Will just use whatever target the targetfinder has if this is set.")]
    public TargetFinder targetFinder;
    
    Rigidbody2D _rigidbody2D;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (targetFinder && targetFinder.currentTarget) 
            target = targetFinder.currentTarget.gameObject;
    }
    
    void FixedUpdate()
    {
        if (!target) return;
        
        Vector2 dir = target.transform.position - transform.position;
        _rigidbody2D.AddForce(dir.normalized * speed.Value * speedMultiplier);
    }
}
