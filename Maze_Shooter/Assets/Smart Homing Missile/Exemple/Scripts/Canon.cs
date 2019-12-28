using UnityEngine;

public class Canon : MonoBehaviour
{
	[SerializeField, Range(0, 10)]
	float m_launchIntensity;

	[SerializeField]
	GameObject m_projectile;

	[SerializeField]
	MissileConfiguration m_config;

	[SerializeField]
	Camera m_cam;

	[SerializeField]
	bool m_forward;
	
	void Update()
	{
		Vector3 targetLook = m_forward
								? m_cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10))
								: m_cam.ScreenToWorldPoint(Input.mousePosition);
		
		transform.parent.LookAt(new Vector3(targetLook.x, targetLook.y, m_forward ? 0 : transform.position.z));

		if (Input.GetMouseButtonDown(0))
			Fire();
	}

	void Fire()
	{
		GameObject newProjectile = Instantiate(m_projectile) as GameObject;
		newProjectile.transform.position = transform.position;

		if (newProjectile.GetComponent<Rigidbody2D>())
		{
			newProjectile.GetComponent<Rigidbody2D>().AddForce(transform.forward * m_launchIntensity,ForceMode2D.Impulse);

			newProjectile.transform.eulerAngles = new Vector3(0,0,-Mathf.Atan2(transform.forward.x, transform.forward.y)*Mathf.Rad2Deg);
			
			SmartMissile2D smartMissile = newProjectile.GetComponent<SmartMissile2D>();
			smartMissile.m_lifeTime = m_config.m_lifeTime;
			smartMissile.m_searchRange = m_config.m_searchRange;
			smartMissile.m_searchAngle = m_config.m_searchAngle;
			smartMissile.m_canLooseTarget = m_config.m_canLooseTarget;
			smartMissile.m_guidanceIntensity = m_config.m_guidanceIntensity;
			smartMissile.m_targetOffset = m_config.m_targetOffset;

			smartMissile.m_distanceInfluence = m_config.m_selectedPreset;
		}
		else if (newProjectile.GetComponent<Rigidbody>())
		{
			newProjectile.GetComponent<Rigidbody>().AddForce(transform.forward * m_launchIntensity, ForceMode.Impulse);

			SmartMissile3D smartMissile = newProjectile.GetComponent<SmartMissile3D>();
			smartMissile.m_lifeTime = m_config.m_lifeTime;
			smartMissile.m_searchRange = m_config.m_searchRange;
			smartMissile.m_searchAngle = m_config.m_searchAngle;
			smartMissile.m_canLooseTarget = m_config.m_canLooseTarget;
			smartMissile.m_guidanceIntensity = m_config.m_guidanceIntensity;
			smartMissile.m_targetOffset = m_config.m_targetOffset;

			smartMissile.m_distanceInfluence = m_config.m_selectedPreset;
		}
	}
}
