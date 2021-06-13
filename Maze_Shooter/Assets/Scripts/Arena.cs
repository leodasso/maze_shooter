using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;

public class Arena : MonoBehaviour
{
	public List<ArenaCircle> arenaCircles = new List<ArenaCircle>();

	public Vector3 CirclePos (ArenaCircle circle) => transform.TransformPoint(circle.offset);

	void OnDrawGizmos() {

		Gizmos.color = new Color(0, 1, 1, .5f);
		foreach (var circle in arenaCircles)
		{
			GizmoExtensions.DrawCircle(CirclePos(circle), circle.radius);
		}
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.cyan;

		foreach (var circle in arenaCircles)
		{
			GizmoExtensions.DrawCircle(CirclePos(circle), circle.radius, true);
		}
	}

	public bool ContainsObject(Transform t)
	{
		for (int i = 0; i < arenaCircles.Count; i++)
		{
			if (Arachnid.Math.IsInRange(t.position - CirclePos(arenaCircles[i]), arenaCircles[i].radius))
				return true;
		}

		return false;
	}

	/// <summary>
	/// Returns a random point within the arena.
	/// </summary>
	public Vector3 GetPoint() {
		int randomIndex = Random.Range(0, arenaCircles.Count);
		ArenaCircle selectedCircle = arenaCircles[randomIndex];

		Vector3 randomPt = Random.insideUnitSphere * selectedCircle.radius;
		Vector3 circlePos = CirclePos(selectedCircle);
		return new Vector3(
			circlePos.x + randomPt.x,
			transform.position.y,
			circlePos.z + randomPt.z
		);
	}

	[Button]
	void Test() {
		for (int i = 0; i < 100; i++){
			Vector3 pt = GetPoint();
			Debug.DrawLine(pt, pt + Vector3.up, Color.red, 5);
		}
	}

	[System.Serializable]
	public class ArenaCircle
	{
		public float radius = 10;

		[HorizontalGroup, LabelText("X"), LabelWidth(20)]
		public float offsetX;
		[HorizontalGroup, LabelText("Z"), LabelWidth(20)]
		public float offsetZ;

		public Vector3 offset => new Vector3(offsetX, 0, offsetZ);
	}
}