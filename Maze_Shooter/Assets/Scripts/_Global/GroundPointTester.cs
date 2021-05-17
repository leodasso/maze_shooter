using UnityEngine;

public class GroundPointTester : MonoBehaviour
{
	void OnDrawGizmosSelected() 
	{
		Vector3 point = GhostTools.GroundPoint(transform.position);

		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, .5f);

		Gizmos.color = Color.cyan;
		Gizmos.DrawLine(transform.position, point);
		Gizmos.DrawWireSphere(point, .25f);
	}
}