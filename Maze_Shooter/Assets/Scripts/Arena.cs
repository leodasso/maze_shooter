using UnityEngine;
using Sirenix.OdinInspector;

public class Arena : MonoBehaviour
{
	public float radius = 10;

	void OnDrawGizmos() {
		Gizmos.color = Color.cyan;
		for (int i = 0; i < 360; i+= 10) {
			Gizmos.DrawLine(PointAtAngle(i), PointAtAngle(i + 10));
		}
	}

	Vector3 PointAtAngle(float angle) {
		float radians = Mathf.Deg2Rad * angle;
		float x = transform.position.x + radius * Mathf.Cos(radians);
		float z = transform.position.z + radius * Mathf.Sin(radians);
		return new Vector3(x, transform.position.y, z);
	}

	public Vector3 GetPoint() {
		Vector3 random = Random.insideUnitSphere * radius;
		return new Vector3(random.x + transform.position.x, transform.position.y, random.z + transform.position.z);
	}

	[Button]
	void Test() {
		Vector3 pt = GetPoint();
		Debug.DrawLine(pt, pt + Vector3.up, Color.red, 5);
	}

	public bool ContainsObject(Transform t) 
	{
		return Vector2.Distance(
			new Vector2(transform.position.x, transform.position.z), 
			new Vector2(t.position.x, t.position.z)) <= radius;
	}
}
