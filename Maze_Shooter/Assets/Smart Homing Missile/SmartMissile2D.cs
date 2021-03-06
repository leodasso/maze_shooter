﻿using UnityEngine;
using Arachnid;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Rigidbody2D))]
public class SmartMissile2D : SmartMissile<Rigidbody2D, Vector2>
{
	void Awake()
	{
		m_rigidbody = GetComponent<Rigidbody2D>();
	}

	protected override Transform findNewTarget()
	{
		if (overrideTarget)
			return customTarget;
		
		foreach (Collider2D newTarget in Physics2D.OverlapCircleAll(transform.position, m_searchRange))
			if ( Math.LayerMaskContainsLayer(targetLayerMask, newTarget.gameObject.layer) && isWithinRange(newTarget.transform.position))
			{
				SetTarget(newTarget.transform);
				return newTarget.transform;
			}

		return null;
	}

	protected override void SetTarget(Transform newTarget)
	{
		m_targetDistance = Vector2.Distance(newTarget.position, transform.position);
		m_target = newTarget;
	}

	protected override bool isWithinRange(Vector3 Coordinates)
	{
		m_forward = m_rigidbody.velocity;
		
		if (Vector2.Distance(Coordinates, transform.position) < m_targetDistance
			&& Vector2.Angle(m_forward, Coordinates - transform.position) < m_searchAngle / 2)
			return true;

		return false;
	}
	
	protected override void goToTarget()
	{		
		m_direction = (m_target.position + (Vector3)m_targetOffset - transform.position).normalized * m_distanceInfluence.Evaluate(1 - (m_target.position + (Vector3)m_targetOffset - transform.position).magnitude / m_searchRange);
		m_rigidbody.velocity = Vector2.ClampMagnitude(m_rigidbody.velocity + m_direction * m_guidanceIntensity, m_rigidbody.velocity.magnitude);
		
		if (m_rigidbody.velocity != Vector2.zero)
		{
			m_forward = m_rigidbody.velocity.normalized;

			if(m_lookDirection)
			{
				transform.eulerAngles = new Vector3(0,0,-Mathf.Atan2(m_rigidbody.velocity.x, m_rigidbody.velocity.y)*Mathf.Rad2Deg);
				transform.Rotate(m_lookDirectionOffset);
			}
		}
	}

#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		if (enabled)
		{
			// Draw the search zone
			Handles.color = m_zoneColor;
			Handles.DrawSolidArc(transform.position, transform.forward, Quaternion.AngleAxis(-m_searchAngle/2, transform.forward) * m_forward, m_searchAngle, m_searchRange);

			// Draw a line to the target
			if (m_target != null)
			{
				Handles.color = m_lineColor;
				Handles.DrawLine(m_target.position + (Vector3)m_targetOffset, transform.position);
			}
		}
	}
#endif
}
