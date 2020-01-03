using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arachnid;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using Math = Arachnid.Math;

[TypeInfoBox("Wobbles the prop back and forth when other bodies collide with this. Lightweight and without physics.")]
public class WobblyProp : MonoBehaviour
{
    [Tooltip("Layers that cause this prop to wobble")]
    public LayerMask layerMask;

    public float wobbliness = 1;
    public float stiffness = 1;
    [Tooltip("Wobble is created by rotating this transform to look at the look point. This value controls the height " +
             "of that point above the position of this transform.")]
    public float lookPointHeight = 5;

    public bool xWobble = true;
    public bool yWobble = true;
    public bool zWobble = true;

    [ReadOnly]
    public Vector3 wobbleVel = Vector3.zero;
    
    [ReadOnly]
    public Vector3 wobblePoint = Vector3.zero;

    Vector3 LookPoint => transform.position + wobblePoint + Vector3.up * lookPointHeight;
    Vector3 LookVector => LookPoint - transform.position;


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        var position = transform.position;
        Gizmos.DrawWireSphere(wobbleVel + position, .3f);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(wobblePoint + position, .1f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(LookPoint, .1f);
        Gizmos.color = new Color(0, 1, 0, .3f);
        Gizmos.DrawLine(wobblePoint + position, LookPoint);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        wobbleVel = Vector3.Lerp(wobbleVel, -wobblePoint, Time.deltaTime * stiffness);
        wobblePoint += wobbliness * Time.deltaTime * wobbleVel;

        transform.rotation = quaternion.LookRotation(Vector3.forward, LookVector);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!Math.LayerMaskContainsLayer(layerMask, other.gameObject.layer))
            return;
        
        DoWobble(other);
    }

    void DoWobble(Collider other)
    {
        // calculate relative velocity
        Vector3 vel = Vector3.zero;
        var otherRb = other.GetComponent<Rigidbody>();
        if (otherRb) vel = otherRb.velocity;
        else
        {
            var otherPseudoVel = other.GetComponent<PseudoVelocity>();
            if (otherPseudoVel) vel = otherPseudoVel.velocity;
        }

        // TODO get total force based on other rigidbody velocity & mass 
        wobbleVel += vel;
    }
}
