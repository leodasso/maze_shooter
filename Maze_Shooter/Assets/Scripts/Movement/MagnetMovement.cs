using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MagnetMovement : MonoBehaviour
{
    [Tooltip("Will have magnetic movement towards objects of this collection")]
    public Collection targets;
    public CurveObject forceOverDistance;
    Rigidbody _rigidbody;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        foreach (var target in targets.elements)
        {
            if (!target) continue;
            if (!target.gameObject.activeInHierarchy) continue;

            Vector3 direction = (target.transform.position - transform.position).normalized;
            _rigidbody.AddForce(direction * MagneticForce(target.transform));
        }
    }

    float MagneticForce(Transform target)
    {
        if (!target) return 0;
        float distance = Vector3.Distance(target.position, transform.position);
        return forceOverDistance.ValueFor(distance);
    }
}
