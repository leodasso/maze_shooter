using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arachnid;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine.Events;

public class PseudoDepth : MonoBehaviour
{
    [Tooltip("Should be a child of this object. This is the actual part that's affected. Collider should NOT" +
             " be part of the visuals, because the visuals are moved to give illusion of depth"), TabGroup("tabGroup", "main")]
    public GameObject visuals;

    [Tooltip("Optional shadow - I will control the opacity and rotation if referenced here."), TabGroup("tabGroup", "main")]
    public GameObject shadow;

    [ShowIf("hasShadow"), Range(0, 1), TabGroup("tabGroup", "main")]
    public float shadowOpacity = .5f;
    
    [Tooltip("How far is this thing from the ground?"), MinValue(0), TabGroup("tabGroup", "main")]
    public float z;
    
    [Tooltip("How much z-depth is there above the visuals origin?"), MinValue(0), TabGroup("tabGroup", "main")]
    public float heightAbove;
    
    [Tooltip("How much z-depth is below the visuals origin?"), MinValue(0), TabGroup("tabGroup", "main")]
    public float heightBelow;

    [Tooltip("Determines how much the y position is offset to give the illusion of z depth"), TabGroup("tabGroup", "main")]
    public FloatReference zFactor;

    [Space] 
    [ToggleLeft, TabGroup("tabGroup", "main")]
    public bool useGravity;
    
    [ShowIf("useGravity"), TabGroup("tabGroup", "main")]
    public FloatReference gravity;

    [MinValue(0), Tooltip("How much will I bounce on the Z-axis? " +
                          "(doesn't affect any other axis or physics material bounce.)"), TabGroup("tabGroup", "main")]
    public float bounce = .15f;
    
    [ShowInInspector, System.NonSerialized, ReadOnly, TabGroup("tabGroup", "main")]
    float zVelocity;

    [Tooltip("Which objects are below me? This determines my 'floor'"), TabGroup("tabGroup", "main")]
    public List<PseudoDepth> belowMe = new List<PseudoDepth>();

    [TabGroup("tabGroup", "events")]
    public UnityEvent onGroundHitEvent;    

    public float globalBottom => z - heightBelow;
    public float globalTop => z + heightAbove;
    public float myFloor()
    {
        float floor = 0;
        foreach (var obj in belowMe)
        {
            if (obj == null) continue;
            // ignore disabled objects
            if (!obj.gameObject.activeInHierarchy) continue;

            floor = Mathf.Max(floor, obj.globalTop);
        }

        return floor;
    }

    float _groundBuffer = .05f;
    bool _grounded = false;
    bool hasShadow => shadow != null;

    SpriteRenderer _shadowSpriteRenderer;


    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 0, .2f);
        DrawHeightGizmo();
        ClampToFloor(myFloor());
        if (!Application.isPlaying) UpdateVisuals();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 0, 1);
        DrawHeightGizmo();
    }

    void DrawHeightGizmo()
    {
        if (!visuals) return;
        // draw the gizmos for dist above and below
        var topPos = transform.position + (Vector3.up * globalTop * zFactor.Value);
        var bottomPos = transform.position + (Vector3.up * globalBottom * zFactor.Value);
        
        DrawDistGizmo(topPos);
        DrawDistGizmo(bottomPos);
    }

    /// <summary>
    /// Draws a gizmo from the visuals origin point to the given position
    /// </summary>
    void DrawDistGizmo(Vector3 pos)
    {
        if (!visuals) return;
        Gizmos.DrawLine(visuals.transform.position, pos);
        float width = .15f;
        Gizmos.DrawLine(new Vector3(pos.x - width, pos.y, pos.z), new Vector3(pos.x + width, pos.y, pos.z));
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float floor = myFloor();
        
        // calculate velocity from gravity
        if (useGravity)
        {
            zVelocity += gravity.Value * Time.deltaTime;
        }

        if (_grounded) zVelocity = Mathf.Clamp(zVelocity, 0, 9999);
        
        // calculate position from velocity
        z += zVelocity * Time.deltaTime;
        
        // don't allow z to be less than the floor
        ClampToFloor(floor);
        
        if (globalBottom <= floor && !_grounded)
        {
            OnHitGround();
        }
        
        // If the z value is greater than the buffer above the ground, we can
        // reset it to ungrounded. The buffer is there so the events don't get called a
        // ton if the z is oscillating right near 0
        else if (globalBottom >= _groundBuffer && _grounded)
        {
            OnLeaveGround();
        }

        UpdateVisuals();
    }
    

    void ClampToFloor(float floor)
    {
        if (globalBottom < floor) z = heightBelow + floor;
    }

    void UpdateVisuals()
    {
        // Update shadow
        if (shadow)
        {
            // shadow rotation should not change with parent
            shadow.transform.rotation = Quaternion.Euler(Vector3.zero);

            float opacity = Mathf.Lerp(shadowOpacity, 0, z / 10);
            if (_shadowSpriteRenderer)
            {
                Color c = _shadowSpriteRenderer.color;
                _shadowSpriteRenderer.color = new Color(c.r, c.g, c.b, opacity);
            }
            else _shadowSpriteRenderer = shadow.GetComponent<SpriteRenderer>();
        }
        
        if ( !visuals) return;
        visuals.transform.position = 
            new Vector3(transform.position.x, transform.position.y + z * zFactor.Value, transform.position.z);
    }

    void OnHitGround()
    {
        _grounded = true;
        Debug.Log(name + " hit the ground!");
        zVelocity = -zVelocity * bounce;
        onGroundHitEvent.Invoke();
    }

    void OnLeaveGround()
    {
        _grounded = false;
        Debug.Log(name + " left the ground!");
    }

    /// <summary>
    /// Returns whether or not this height is overlapping the other pseudodepth's height
    /// </summary>
    public bool OverlapWith(PseudoDepth other)
    {
        return globalTop >= other.globalBottom && globalBottom < other.globalTop;
    }

    public bool OverlapWith(float depth)
    {
        return globalTop >= depth && globalBottom < depth;
    }

    public bool DefaultOverlap()
    {
        return OverlapWith(0) || OverlapWith(.5f) || OverlapWith(1);
    }
}
