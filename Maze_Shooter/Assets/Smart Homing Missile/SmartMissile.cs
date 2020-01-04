using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public abstract class SmartMissile : MonoBehaviour { }

public abstract class SmartMissile<RgbdType, VecType> : SmartMissile
{
	[Header("Missile")]
	public bool hasLifetime;

	[SerializeField, Tooltip("In seconds, 0 for infinite lifetime."), ShowIf("hasLifetime")]
	public float lifeTime = 5;
	
	[SerializeField, Tooltip("Rotate the missile so it's looking the direction it's moving")]
	protected bool m_lookDirection = true;
	
	[SerializeField, ShowIf("m_lookDirection")]
	protected Vector3 m_lookDirectionOffset;
	
	[SerializeField]
	UnityEvent m_onNewTargetFound;
	
	[SerializeField]
	UnityEvent m_onTargetLost;

	[Header("Detection")]
	
	[SerializeField, Tooltip("Range within the missile will search a new target.")]
	public float m_searchRange = 10f;
	[SerializeField, Range(0, 360)]
	public int m_searchAngle = 90;
	[SerializeField, Tooltip("If enabled, target is lost when out of the range.")]
	public bool m_canLooseTarget = true;

	[Header("Guidance")]
	[SerializeField, Tooltip("Intensity the missile will be guided with.")]
	public float m_guidanceIntensity = 5f;
	[SerializeField, Tooltip("Increase the intensity depending on the distance.")]
	public AnimationCurve m_distanceInfluence = AnimationCurve.Linear(0, 1, 1, 1);

	[Header("Target")]
	[SerializeField, Tooltip("Use this if the center of the object is not what you target.")]
	public VecType m_targetOffset;
	[Tooltip("Allows you to set a custom target, and turns off target-finding behavior.")]
	public bool overrideTarget;
	[ShowIf("overrideTarget")]
	public Transform customTarget;
	[HideIf("overrideTarget")]
	public LayerMask targetLayerMask;

	[Header("Debug")]
	[SerializeField, Tooltip("Color of the search zone."), HideInInspector]
	protected Color m_zoneColor = new Color(255, 0, 155, 0.1f);
	[SerializeField, Tooltip("Color of the line to the target."), HideInInspector]
	protected Color m_lineColor = new Color(255, 0, 155, 1);

	protected RgbdType m_rigidbody;
	protected Transform m_target;
	protected float m_targetDistance;
	protected VecType m_direction;
	protected Vector3 m_forward;

	void Start()
	{
		m_targetDistance = m_searchRange;

		if (lifeTime > 0 && hasLifetime)
			Destroy(gameObject, lifeTime);
	}

	void FixedUpdate()
	{
		if (m_target != null)
		{
			if (m_canLooseTarget && !isWithinRange(m_target.transform.position))
			{
				m_target = null;
				m_targetDistance = m_searchRange;
				m_onTargetLost.Invoke();
			}
			else
			{
				goToTarget();
			}
		}
		else if (m_target = findNewTarget())
			m_onNewTargetFound.Invoke();
	}

	public void ClearTarget()
	{
		m_target = null;
	}

	void OnEnable()
	{
		ClearTarget();
	}

	/// <summary>
	/// Find a new target within the search zone. Returns null if no target is found.
	/// </summary>
	protected abstract Transform findNewTarget();
	
	/// <summary>
	/// Returns true if the input Coodinates are within the search zone.
	/// </summary>
	protected abstract bool isWithinRange(Vector3 coordinates);

	/// <summary>
	/// Update the direction of the Rigidbody.
	/// </summary>
	protected abstract void goToTarget();
}