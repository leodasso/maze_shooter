using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileConfiguration : MonoBehaviour
{
	// Missile
	public float m_lifeTime;

	// Detection
	public float m_searchRange;
	public int m_searchAngle;
	public bool m_canLooseTarget;

	// Guidance
	public float m_guidanceIntensity;
	public AnimationCurve m_distanceInfluence1;
	public AnimationCurve m_distanceInfluence2;
	public AnimationCurve m_distanceInfluence3;
	public AnimationCurve m_selectedPreset;

	// Target
	public Vector3 m_targetOffset = new Vector3();

	void Start()
	{
		m_selectedPreset = m_distanceInfluence1;
	}

	public void SetLifeTime(float input)
	{
		m_lifeTime = input;
	}

	public void SetSearchRange(float input)
	{
		m_searchRange = input;
	}

	public void SetAngle(float input)
	{
		m_searchAngle = (int)input;
	}

	public void SetCanLooseTarget(bool input)
	{
		m_canLooseTarget = input;
	}

	public void Setintensity(float input)
	{
		m_guidanceIntensity = input;
	}

	public void SetOffsetX(float input)
	{
		m_targetOffset.x = input;
	}

	public void SetOffsetY(float input)
	{
		m_targetOffset.y = input;
	}

	public void SetOffsetZ(float input)
	{
		m_targetOffset.z = input;
	}

	public void SetPreset(int input)
	{
		switch (input)
		{
			case 0:
				m_selectedPreset = m_distanceInfluence1;
			break;
			case 1:
				m_selectedPreset = m_distanceInfluence2;
			break;
			case 2:
				m_selectedPreset = m_distanceInfluence3;
			break;
		}
	}
}
