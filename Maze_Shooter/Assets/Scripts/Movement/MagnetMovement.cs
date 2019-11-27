using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MagnetMovement : MonoBehaviour
{
    [Tooltip("Will have magnetic movement towards objects of this collection")]
    public Collection targets;
    public CurveObject forceOverDistance;
    Rigidbody2D _rigidbody2D;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        foreach (var target in targets.elements)
        {
            if (!target) continue;
            if (!target.gameObject.activeInHierarchy) continue;

            Vector2 direction = (target.transform.position - transform.position).normalized;
            _rigidbody2D.AddForce(direction * MagneticForce(target.transform));
        }
    }

    float MagneticForce(Transform target)
    {
        if (!target) return 0;
        float distance = Vector3.Distance(target.position, transform.position);
        return forceOverDistance.ValueFor(distance);
    }
}
