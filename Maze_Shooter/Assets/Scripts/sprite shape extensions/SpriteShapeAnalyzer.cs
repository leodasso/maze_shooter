using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using Sirenix.OdinInspector;



public struct ShapePoint 
{
	public Vector3 pos;
	public float height;

	public ShapePoint(Vector3 newPos, float newHeight)
	{
		pos = newPos;
		height = newHeight;
	}
}




public class SpriteShapeAnalyzer : MonoBehaviour
{
	public SpriteShapeController spriteShapeController;

	[Space]
	[Range(1, 30), Tooltip("The number of segments to break curved pieces into"), OnValueChanged("Analyze")]
	public int curveSegments = 5;

	[Range(0, .1f), Tooltip("Higher numbers means it will remove minor details"), OnValueChanged("Analyze")]
	public float simplify = .005f;

	[Tooltip("How much the height of the sprite shape points is taken into account")]
	public float heightFactor = 2;

	[ReadOnly, Space, PropertyOrder(200)]
	public List<ShapePoint> points = new List<ShapePoint>();

	public bool IsOpenEnded => spriteShapeController ? spriteShapeController.spline.isOpenEnded : false;



	void OnDrawGizmosSelected()
	{
		if (!spriteShapeController) return;

		Gizmos.color = Color.cyan;
		for ( int i = 1; i < points.Count; i++) {

			Vector3 p1 = transform.TransformPoint(points[i-1].pos);
			Vector3 p2 = transform.TransformPoint(points[i].pos);
			Gizmos.DrawLine(p1, p2);

			Vector3 vector = p2 - p1;
			Vector3 right = Vector3.Cross(vector, Vector3.up).normalized * heightFactor * points[i-1].height;
			Vector3 left = -right;

			Gizmos.DrawWireSphere(p1 + left, .25f);
			Gizmos.DrawWireSphere(p1 + right, .25f);
		}
	}


	/// <summary>
	/// This component gives the option to simplify the path. If there are two segments that have a nearly identical angle,
	/// simplify will just turn that into one segment. This function returns all the indexes of the path after it's been simplified.
	/// </summary>
	List<int> UsableIndexes()
	{
		List<int> usable = new List<int>();

		// Add first point
		usable.Add(0);

		int pointCount = spriteShapeController.spline.GetPointCount();
		for (int i = 1; i < pointCount - 1; i++) {

			Vector3 pt1 = spriteShapeController.spline.GetPosition(i-1);
			Vector3 pt2 = spriteShapeController.spline.GetPosition(i);
			Vector3 pt3 = spriteShapeController.spline.GetPosition(i + 1);

			Vector3 prevDirection = pt2 - pt1;
			Vector3 nextDirection = pt3 - pt2;
			float dot = Vector3.Dot(prevDirection.normalized, nextDirection.normalized);

			if (Mathf.Abs(dot) < 1 - simplify) 
				usable.Add(i);
		}

		// add last point
		usable.Add(pointCount - 1);
		return usable;
	}

	[Button]
	/// <summary>
	/// Analyzes the sprite shape and generates a list of usable points.
	/// </summary>
	public void Analyze()
	{	
		points.Clear();
		points = new List<ShapePoint>();

		// Generate a list of usable indexes. Basically, ignore ones that make lines that are ALMOST the same direction
		var usable = UsableIndexes();

		for (int i = 1; i < usable.Count; i++) 
				GenerateSingleSegment(usable[i-1], usable[i]);
	
		// build the very last collider from last point to the first point
		if (!spriteShapeController.spline.isOpenEnded) 
			GenerateSingleSegment(usable[usable.Count - 1], usable[0]);
	}

	static ShapePoint GetShapePoint(SpriteShapeController spriteShape, int index)
		=> new ShapePoint(
			spriteShape.spline.GetPosition(index),
			spriteShape.spline.GetHeight(index)
		);


	void GenerateSingleSegment(int leftIndex, int rightIndex)
	{
		//failsafe for creating infinite points
		if (curveSegments < 1) return;

		ShapePoint pt1 = GetShapePoint(spriteShapeController, leftIndex);
		ShapePoint pt2 = GetShapePoint(spriteShapeController, rightIndex);

		Vector3 tangent1 = spriteShapeController.spline.GetRightTangent(leftIndex);
		Vector3 tangent2 = spriteShapeController.spline.GetLeftTangent(rightIndex);

		// check if this is curved
		bool curved = tangent1.magnitude > .1f || tangent2.magnitude > .1f;

		if (! curved) {
			points.Add(pt1);
			points.Add(pt2);
			return;
		}

		// generate curve
		Vector3 anchor1 = pt1.pos + tangent1;
		Vector3 anchor2 = pt2.pos + tangent2;

		float segmentLength = 1f / (float)curveSegments;

		for (float i = 0; i < 1; i += segmentLength){
			float height = Mathf.Lerp(pt1.height, pt2.height, i);
			Vector3 ptPos = Arachnid.Math.GetBezier(i, pt1.pos, anchor1, anchor2, pt2.pos);
			points.Add(new ShapePoint(ptPos, height));
		}
	}

	[Button]
	public void Reset()
	{
		points.Clear();
		points = new List<ShapePoint>();
	}
}