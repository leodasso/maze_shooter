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

	[ToggleLeft, Tooltip("Use noise to gently wobble always. This has a performance hit if there are thousands of instances.")]
	public bool useNoise;

    [Tooltip("(optional) Perlin noise profile, for adding ambient wobble, like wind blowing."), ShowIf("useNoise")]
    public NoiseProfile noiseProfile;

    [ToggleLeft, Tooltip("The default object to wobble is this. Use this toggle if you want the collisions/triggers from this object " +
             "to wobble another object.")]
    public bool wobbleOtherObject;

    [ShowIf("wobbleOtherObject"), Indent]
    public Transform wobbler;
    
    [ShowIf("HasNoise")]
    public float noiseMultiplier = 1;
    public float wobbliness = 1;
    public float stiffness = 1;
    public Vector3 wobbleScale = Vector3.one;
	[Tooltip("When something triggers this, its perceived velocity will be clamped to max velocity.")]
	public float maxVelocity = 10;

    [ReadOnly]
    public Vector3 wobbleVel = Vector3.zero;
    
    [ReadOnly]
    public Vector3 wobblePoint = Vector3.zero;

    Quaternion initRotation;

    bool HasNoise => useNoise && noiseProfile != null;

    Vector2 _noiseScrollPoint;
    Vector2 _currentNoiseValue;
    Transform ObjectToWobble => wobbleOtherObject ? wobbler : transform;


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
        initRotation = ObjectToWobble.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
		// Improve performance by only calculating when its moving
		if (wobbleVel.sqrMagnitude < .01f && !HasNoise) return;

		// Lerp the wobble velocity to the negative wobble point
        wobbleVel = Vector3.Lerp(wobbleVel, -wobblePoint, Time.deltaTime * stiffness);

		// Add the wobble velocity to the wobble point 
        wobblePoint += wobbliness * Time.deltaTime * wobbleVel;

		// scale the wobble point based on the exposed wobbleScale (to give more design/control)
        Vector3 scaledWobblePoint = Vector3.Scale(wobbleScale, wobblePoint);
        
        if (HasNoise)
        {
            _noiseScrollPoint += Time.deltaTime * noiseProfile.scrollSpeed;
            _currentNoiseValue = GeneratedPerlinVector() * noiseMultiplier * noiseProfile.strength;
        }
        
        Quaternion rot = Quaternion.Euler(
			scaledWobblePoint.z + _currentNoiseValue.x, 	// X
			scaledWobblePoint.y, 							// Y
			-scaledWobblePoint.x + _currentNoiseValue.y);	// Z

        ObjectToWobble.localRotation = initRotation * rot;
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
        
        SetWobble(GetVelocity(other.gameObject));
    }

    void OnCollisionEnter(Collision other)
    {
        if (!Math.LayerMaskContainsLayer(layerMask, other.gameObject.layer))
            return;
        
        SetWobble(other.relativeVelocity);
    }

    Vector3 GetVelocity(GameObject other)
    {
        var otherRb = other.GetComponent<Rigidbody>();
        if (otherRb) 
            return otherRb.velocity;

        var otherPseudoVel = other.GetComponent<PseudoVelocity>();
        if (otherPseudoVel) 
            return otherPseudoVel.velocity;

        return Vector3.zero;
    }

    public void SetWobble(Vector3 vector)
    {
        wobbleVel += Vector3.ClampMagnitude( Vector3.Scale(vector, transform.localScale), maxVelocity);
    }
}
