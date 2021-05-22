using UnityEngine;
using Sirenix.OdinInspector;

public class Arena : MonoBehaviour
{
	public float radius = 10;

	void OnDrawGizmos() {
		Gizmos.color = Color.cyan;
		GizmoExtensions.DrawCircle(transform.position, radius);
	}

	/// <summary>
	/// Returns a random point within the arena.
	/// </summary>
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
