using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Sirenix.OdinInspector;

[ExecuteAlways]
public class PathFollower : MovementBase
{
    public float acceleration = 10;
    public float drag = 10;
    public CinemachineSmoothPath path;
    [Tooltip("If true, my movement will take priority over any other pathFollowers I encounter. Enable this for a " +
             "player controlled path follower.")]
    public bool master;
    public float lerpSpeed = 10;
    public float pathPosition;
    public float radius = 1;
    [Range(1, 5), Tooltip("How much can the angle of input differ from angle of path and still achieve full speed?")]
    public float inputDirectionForgiveness = 2;
    public List<PathFollower> siblings = new List<PathFollower>();

    bool _slowDown;
    Vector3 _pathTangent;
    CinemachinePathBase.PositionUnits _units = CinemachinePathBase.PositionUnits.Distance;
    
    /// <summary>
    /// The dot product between the path tangent and the movement input direction.
    /// </summary>
    float _dot;
    float MaxSpeed => speed.Value * speedMultiplier;
    float _speedOnPath;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, _pathTangent * 1);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    // Update is called once per frame
    void Update()
    {
        if (!path) return;
        _pathTangent = path.EvaluateTangentAtUnit(pathPosition, _units);

        if (master) AutonomousUpdate();

        pathPosition += _speedOnPath * Time.deltaTime;
        // Loop the path position so it stays within the bounds of path length.
        if (pathPosition < 0)
            pathPosition += path.PathLength;
        else if (pathPosition > path.PathLength)
            pathPosition -= path.PathLength;
        
        // Lerp me to the path point
        transform.position = Vector3.Lerp(transform.position, path.EvaluatePositionAtUnit(pathPosition, _units), 
            Time.deltaTime * lerpSpeed);
    }

    void AutonomousUpdate()
    {
        _dot = Vector3.Dot(_pathTangent.normalized, direction.normalized) * inputDirectionForgiveness;
        _dot = Mathf.Clamp(_dot, -1, 1);
        _speedOnPath += acceleration * _dot * direction.magnitude;
        _speedOnPath = Mathf.Clamp(_speedOnPath, -MaxSpeed, MaxSpeed);

        _speedOnPath = Mathf.Lerp(_speedOnPath, 0, Time.deltaTime * drag);

        foreach (var sibling in siblings)
        {
            sibling._speedOnPath = _speedOnPath;
        }
    }

    [Button]
    void ZeroSpeed()
    {
        _speedOnPath = 0;
    }
    
    void OnCollisionEnter(Collision other)
    {
        PathFollower otherPathFollower = other.collider.GetComponent<PathFollower>();
        if (otherPathFollower)
        {
            // TODO unity event when bumping into another
        }
        
        Debug.Log("Collided with something homie");
    }


    [Button]
    void GetSiblings()
    {
        siblings.Clear();
        Transform parent = transform.parent;
        if (!parent)
        {
            Debug.LogWarning(name + " has no parent, so can't get siblings! If you're trying to group path followers " +
                             "together, try placing them all in an empty game object.");
            return;
        }
        
        siblings.AddRange(parent.GetComponentsInChildren<PathFollower>());
        siblings.Remove(this);
    }

    // Below functions are for easy interfacing with PlayMaker
    public void SetAsMaster()
    {
        master = true;
    }

    public void UnMaster()
    {
        master = false;
    }
}
