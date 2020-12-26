using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;
using Sirenix.OdinInspector;

[ExecuteAlways]
public class PathFollower : MovementBase
{
    [Tooltip("If true, my movement will take priority over any other pathFollowers I encounter. Enable this for a " +
             "player controlled path follower."), PropertyOrder(-999)]
    public bool master;

    [Space]
    public float acceleration = 10;
    public float drag = 10;
	public float maxSpeed = 5;
    [Tooltip("Speed of lerping position to the current point along the path.")]
    public float lerpSpeed = 10;
    public float pathPosition;

	public bool clampPathPos;
	public bool orientToPath;

	[ShowIf("clampPathPos"), HorizontalGroup("clamp"), LabelText("Min"), LabelWidth(40)]
	public float minPathPos;

	[ShowIf("clampPathPos"), HorizontalGroup("clamp"), LabelText("Max"), LabelWidth(40)]
	public float maxPathPos;
    
    [Tooltip("Try to get it to match the collider approximately - this is used for faking collision.")]
    public float radius = 2;
    
    [Range(1, 5), Tooltip("How much can the angle of input differ from angle of path and still achieve full speed?")]
    public float inputDirectionForgiveness = 2;
    
    public LayerMask collisionLayerMask;
    
    [Space]
    public CinemachineSmoothPath path;

    bool _slowDown;
    Vector3 _pathTangent;
    CinemachinePathBase.PositionUnits _units = CinemachinePathBase.PositionUnits.Distance;

	public float PathSpeed => _speedOnPath;
	public float FinalRadius => radius * transform.localScale.x;
    
    /// <summary>
    /// The dot product between the path tangent and the movement input direction.
    /// </summary>
    float _dot;
    float _speedOnPath;

	public void PlaceAtEnd() {
		pathPosition = path.PathLength;
	}

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, _pathTangent * 1);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, FinalRadius);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        
        if (!path) return;
        _pathTangent = path.EvaluateTangentAtUnit(pathPosition, _units);

        if (master)
			MasterUpdate();

		UpdatePosition();

    }

	void UpdatePosition() 
	{
        pathPosition += _speedOnPath * Time.deltaTime;

		if (clampPathPos)
			pathPosition = Mathf.Clamp(pathPosition, minPathPos, maxPathPos);
		
        // Loop the path position so it stays within the bounds of path length.
		if (path.Looped) {
			if (pathPosition < 0)
				pathPosition += path.PathLength;
			else if (pathPosition > path.PathLength)
				pathPosition -= path.PathLength;
		}else {
			pathPosition = Mathf.Clamp(pathPosition, 0, path.PathLength);
		}

		if (orientToPath) {
			transform.rotation = path.EvaluateOrientationAtUnit(pathPosition, _units);
		}
        
        // Lerp me to the path point
        transform.position = Vector3.Lerp(transform.position, path.EvaluatePositionAtUnit(pathPosition, _units), 
            Time.deltaTime * lerpSpeed);
	}

    void MasterUpdate()
    {
        _dot = Vector3.Dot(_pathTangent.normalized, direction.normalized) * inputDirectionForgiveness;
        _dot = Mathf.Clamp(_dot, -1, 1);
        _speedOnPath += acceleration * _dot * direction.magnitude * Time.deltaTime;
        _speedOnPath = Mathf.Clamp(_speedOnPath, -maxSpeed, maxSpeed);

		if (direction.magnitude < .1f)
        	_speedOnPath = Mathf.Lerp(_speedOnPath, 0, Time.deltaTime * drag);
    }

    [Button]
    public void ZeroSpeed()
    {
        _speedOnPath = 0;
    }
    
    protected override void OnCollisionEnter(Collision other)
    {
		base.OnCollisionEnter(other);

        PathFollower otherPathFollower = other.collider.GetComponent<PathFollower>();
        if (otherPathFollower)
        {
            // TODO unity event when bumping into another
        }
        
        if (other.collider.isTrigger) return;
        if (!Arachnid.Math.LayerMaskContainsLayer(collisionLayerMask, other.gameObject.layer)) return;

        Vector3 groundPoint = other.GetContact(0).point;
        groundPoint = new Vector3(groundPoint.x, transform.position.y, groundPoint.z);
        Debug.DrawLine(groundPoint, groundPoint + Vector3.up, Color.magenta, 30);
        
        // whichever direction we're currently moving along the path, we can assume the way we want to move
        // when colliding is the opposite of that
        float correctionDir = _speedOnPath > 0 ? -1 : 1;
        
        // Prevent overlaps
        int iterations = 0;
        float currentPathPos = pathPosition;
        // Because distance calc is expensive, limit this to only a few iterations
        while (iterations < 5)
        {
            currentPathPos += FinalRadius / 5 * correctionDir;
            if (Vector3.Distance(groundPoint, path.EvaluatePositionAtUnit(currentPathPos, _units)) > FinalRadius)
                break;
            iterations++;
        }
        
        // manually place me at the new non-overlapping position
        pathPosition = currentPathPos;
        transform.position = path.EvaluatePositionAtUnit(pathPosition, _units);
        
        Bounce();
    }

    void Bounce()
    {
        _speedOnPath *= -.9f;
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
