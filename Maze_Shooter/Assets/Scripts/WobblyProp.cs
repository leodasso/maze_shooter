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

    [Tooltip("(optional) Perlin noise profile, for adding ambient wobble, like wind blowing.")]
    public NoiseProfile noiseProfile;
    
    [ShowIf("HasNoise")]
    public float noiseMultiplier = 1;

    public float wobbliness = 1;
    public float stiffness = 1;
    public Vector3 wobbleScale = Vector3.one;

    [ReadOnly]
    public Vector3 wobbleVel = Vector3.zero;
    
    [ReadOnly]
    public Vector3 wobblePoint = Vector3.zero;

    Quaternion initRotation;

    bool HasNoise => noiseProfile != null;

    Vector2 _noiseScrollPoint;
    Vector2 _currentNoiseValue;


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        var position = transform.position;
        Gizmos.DrawWireSphere(wobbleVel + position, .3f);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(wobblePoint + position, .1f);
    }

    // Start is called before the first frame update
    void Start()
    {
        initRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        wobbleVel = Vector3.Lerp(wobbleVel, -wobblePoint, Time.deltaTime * stiffness);
        wobblePoint += wobbliness * Time.deltaTime * wobbleVel;
        Vector3 scaledWobblePoint = Vector3.Scale(wobbleScale, wobblePoint);
        
        if (HasNoise)
        {
            _noiseScrollPoint += Time.deltaTime * noiseProfile.scrollSpeed;
            _currentNoiseValue = GeneratedPerlinVector() * noiseMultiplier * noiseProfile.strength;
        }
        
        Quaternion rot = Quaternion.Euler(scaledWobblePoint.z + _currentNoiseValue.x, scaledWobblePoint.y, -scaledWobblePoint.x + _currentNoiseValue.y);
        transform.localRotation = initRotation * rot;
    }
    
    Vector2 GeneratedPerlinVector()
    {
        float inputX = transform.position.x + _noiseScrollPoint.x;
        float inputY = transform.position.y + _noiseScrollPoint.y;
        
        float x = Mathf.PerlinNoise(inputX * noiseProfile.frequency, inputY * noiseProfile.frequency) - .5f;
        float y = Mathf.PerlinNoise(inputY * noiseProfile.frequency, inputX * noiseProfile.frequency) - .5f;
        return new Vector2(x, y);
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
