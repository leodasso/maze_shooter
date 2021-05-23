using UnityEngine;
using Sirenix.OdinInspector;
using Arachnid;
using System.Collections.Generic;

public class SpriteShapeCollider : MonoBehaviour
{
	public enum ColliderLayer {
		Default,
		Terrain,
		SoulLight,
	}

	[EnumToggleButtons, OnValueChanged("UpdateLayer")]
	public ColliderLayer colliderLayer;

	[ToggleLeft, OnValueChanged("SetIsTrigger")]
	public bool isTrigger;

	[ToggleLeft, Tooltip("Fill the shape with voxel colliders")]
	public bool filled;

	[Tooltip("Width of voxel bars"), TitleGroup("Voxel Size", "Width / Height")]
	[MinValue(1f), ShowIf("filled"), HideLabel, HorizontalGroup("Voxel Size/voxels")]
	public float voxelWidth = 5;

	[TitleGroup("Voxel Size")]
	[MinValue(.5f), ShowIf("filled"), HideLabel, HorizontalGroup("Voxel Size/voxels")]
	public float voxelHeight = .5f;

	public float height = 5;

	[Range(1, 30), Tooltip("The number of segments to break curved pieces into")]
	public int curveSegments = 5;

	[Tooltip("Hint: To make a trigger that more or less matches this sprite shape, just set the thickness to a negative number.")]
	public float thickness = 1;
	public UnityEngine.U2D.SpriteShapeController spriteShapeController;

	[SerializeField, ReadOnly]
	List<Vector3> points = new List<Vector3>();

	[SerializeField, ReadOnly]
	List<GameObject> walls = new List<GameObject>();

	[SerializeField, ReadOnly]
	List<GameObject> voxels = new List<GameObject>();


	void OnDrawGizmosSelected()
	{
		var bounds = GetBounds();
		Gizmos.DrawWireCube(bounds.center, bounds.size);

		Gizmos.color = Color.cyan;
		for ( int i = 1; i < points.Count; i++)
			Gizmos.DrawLine(transform.TransformPoint(points[i-1]), transform.TransformPoint(points[i]));
	}


	Bounds GetBounds() 
	{
		GenerateSegments();
		Bounds bounds = new Bounds(transform.position, Vector3.one);

		for (int i = 0; i < points.Count; i++) {
			Vector3 pt = points[i];
			bounds.Encapsulate(transform.TransformPoint(pt));
		}

		bounds.size += new Vector3(voxelWidth * 2, 0, voxelHeight * 2);

		return bounds;
	}

	void GenerateSegments()
	{
		points.Clear();
		int pointCount = spriteShapeController.spline.GetPointCount();

		for (int i = 1; i < pointCount; i++) 
			GenerateSingleSegment(i-1, i);
	
		// build the very last collider from last point to the first point
		if (!spriteShapeController.spline.isOpenEnded) 
			GenerateSingleSegment(pointCount - 1, 0);
	}

	void GenerateSingleSegment(int leftIndex, int rightIndex)
	{
		//failsafe for creating infinite points
		if (curveSegments < 1) return;

		Vector3 pt1 = spriteShapeController.spline.GetPosition(leftIndex);
		Vector3 pt2 = spriteShapeController.spline.GetPosition(rightIndex);

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
		Vector3 anchor1 = pt1 + tangent1;
		Vector3 anchor2 = pt2 + tangent2;

		float segmentLength = 1f / (float)curveSegments;

		for (float i = 0; i < 1; i += segmentLength){
			points.Add(Math.GetBezier(i, pt1, anchor1, anchor2, pt2));
		}
	}
	
	[ButtonGroup]
	void BuildCollider()
	{
		ClearAllColliders();
		GenerateSegments();
		
		if (!filled)
			BuildWalls(thickness);

		if (filled)
			BuildFill();

		SetIsTrigger();

		UpdateLayer();
	}

	void UpdateLayer()
	{
		SetLayer(colliderLayer.ToString());
	}

	void SetLayer(string layerName)
	{
		foreach (var wall in walls)
			wall.layer = LayerMask.NameToLayer(layerName);

		foreach (var voxel in voxels)
			voxel.layer = LayerMask.NameToLayer(layerName);
	}

	void BuildWalls(float thickness) 
	{
		for (int i = 1; i < points.Count; i++) 
			BuildColliderSegment(points[i-1], points[i], i, thickness);

		// build the very last collider from last point to the first point
		if (!spriteShapeController.spline.isOpenEnded) 
			BuildColliderSegment(points[points.Count - 1], points[0], points.Count, thickness);
	}

	[Button]
	void BuildFill()
	{
		float prevThickness = thickness;

		RemoveWalls();

		// make thin borders so we can use them to calculate
		BuildWalls(.01f);

		Bounds bounds = GetBounds();
		Vector3 voxelPos = FlatVoxelPos(bounds.min);

		// iterate X
		while (voxelPos.x < bounds.max.x) {

			voxelPos = new Vector3(voxelPos.x, voxelPos.y, bounds.min.z);
			Vector3 prev = voxelPos;

			bool inside = false;
			bool previouslyInside = false;

			// iterate z
			while (voxelPos.z < bounds.max.z) {

				// raycast from prev point to next
				foreach (var hit in Physics.RaycastAll(prev, voxelPos - prev, voxelHeight, LayerMask.GetMask("Default", "Terrain"))) {
					
					// if the parent of the hit isnt this, we can ignore it
					if (hit.transform.parent != transform) continue;
					// ignore other voxels
					if (voxels.Contains(hit.transform.gameObject)) continue;

					// flip inside status
					inside = !inside;
				}

				// create voxel collider
				if (inside) {

					// if we were previously inside too, just extend the previous box collider
					// this way we can reduce the total number of box colliders needed
					if (previouslyInside) {
						GameObject previousVoxel = voxels[voxels.Count - 1];
						BoxCollider boxCol = previousVoxel.GetComponent<BoxCollider>();
						boxCol.size += Vector3.forward * voxelHeight;
						boxCol.center += Vector3.forward * voxelHeight/2;
					}

					else {
						GameObject newVoxel = new GameObject("voxel");
						newVoxel.transform.parent = transform;
						newVoxel.transform.position = FlatVoxelPos(voxelPos);
						var boxCol = newVoxel.AddComponent<BoxCollider>();
						boxCol.size = new Vector3(voxelWidth, height, voxelHeight);
						voxels.Add(newVoxel);
					}
				}

				previouslyInside = inside;
				prev = voxelPos;
				voxelPos += Vector3.forward * voxelHeight;
			}
			voxelPos += Vector3.right * voxelWidth;
		}

		thickness = prevThickness;
		BuildWalls(prevThickness);
	}

	Vector3 FlatVoxelPos (Vector3 input) => new Vector3(input.x, transform.position.y + height / 2, input.z);

	[ButtonGroup]
	void ClearAllColliders()
	{
		RemoveWalls();
		RemoveVoxels();
	}

	void RemoveWalls()
	{
		for (int i = 0; i < walls.Count; i++) {
			if (walls[i] != null) 
				DestroyImmediate(walls[i]);
		}
		walls.Clear();
	}

	void RemoveVoxels()
	{
		for (int i = 0; i < voxels.Count; i++) {
			if (voxels[i] != null) 
				DestroyImmediate(voxels[i]);
		}
		voxels.Clear();
	}

	/// <summary>
	/// For editor use only!
	/// </summary>
	void SetIsTrigger()
	{
		foreach (GameObject collider in walls)
		{
			if (!collider) continue;
			collider.GetComponent<Collider>().isTrigger = isTrigger;
		}

		foreach (GameObject col in voxels)
		{
			if (!col) continue;
			col.gameObject.GetComponent<Collider>().isTrigger = isTrigger;
		}
	}

	void BuildColliderSegment(Vector3 pt1, Vector3 pt2, int index, float thickness) 
	{
		// calculate the angle to rotate the collider to
		Vector3 offset = pt2 - pt1;
		float angle = Math.AngleFromVector2(offset, -90);

		GameObject newCol = new GameObject("col " + index);
		newCol.transform.parent = transform;
		walls.Add(newCol);

		var newBox = newCol.AddComponent<BoxCollider>();

		float segmentLength = Vector3.Distance(pt1, pt2);
		newBox.size = new Vector3(segmentLength, height, thickness);
		newBox.center = new Vector3(segmentLength / 2, height / 2, thickness/2);
		newBox.transform.localPosition = pt1;
		newBox.transform.localEulerAngles = new Vector3(angle, -90, 90);
	}
}