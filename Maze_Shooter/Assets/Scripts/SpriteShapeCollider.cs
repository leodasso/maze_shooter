using UnityEngine;
using UnityEngine.U2D;
using Sirenix.OdinInspector;
using Arachnid;
using System.Collections.Generic;

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

public class SpriteShapeCollider : MonoBehaviour
{


	public SpriteShapeController spriteShapeController;
	
	public enum ColliderLayer {
		Default,
		Terrain,
		SoulLight,
	}

	[EnumToggleButtons, OnValueChanged("UpdateLayer"), Space]
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

	[Range(1, 30), Tooltip("The number of segments to break curved pieces into"), TitleGroup("Edges")]
	public int curveSegments = 5;

	[Range(0, .1f), Tooltip("Higher numbers means it will remove minor details")]
	public float simplify = .005f;

	[Tooltip("Thickness of the border colliders.")]
	public float thickness = 1;

	[Tooltip("SpriteShape has something called 'height', whcih is the thickness of the border. This controls how much that height affects the total thickness of the collider.")]
	public float thicknessFromShape = 1;

	[Range(-.5f, .5f), Tooltip("Offset of the collider from the path line. 0 means the collider will be centered over the path line.")]
	public float offset = .5f;

	[Range(-.05f, 1), Tooltip("How much edge colliders overlap as a percentage")]
	public float overlap = .15f;

	[SerializeField, ReadOnly, Space, PropertyOrder(200)]
	List<ShapePoint> points = new List<ShapePoint>();

	[SerializeField, ReadOnly, PropertyOrder(200)]
	List<GameObject> walls = new List<GameObject>();

	[SerializeField, ReadOnly, PropertyOrder(200)]
	List<GameObject> voxels = new List<GameObject>();


	void OnDrawGizmosSelected()
	{
		if (!spriteShapeController) return;

		var bounds = GetBounds();
		Gizmos.DrawWireCube(bounds.center, bounds.size);

		Gizmos.color = Color.cyan;
		for ( int i = 1; i < points.Count; i++)
			Gizmos.DrawLine(transform.TransformPoint(points[i-1].pos), transform.TransformPoint(points[i].pos));
	}


	Bounds GetBounds() 
	{
		GenerateSegments();
		Bounds bounds = new Bounds(transform.position, Vector3.one);

		for (int i = 0; i < points.Count; i++) {
			Vector3 pt = points[i].pos;
			bounds.Encapsulate(transform.TransformPoint(pt));
		}

		bounds.size += new Vector3(voxelWidth * 2, 0, voxelHeight * 2);

		return bounds;
	}

	// ignore points if the difference between the two segments is really negligable (as determined by wiggleRoom)
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

	void GenerateSegments()
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
			Vector3 ptPos = Math.GetBezier(i, pt1.pos, anchor1, anchor2, pt2.pos);
			points.Add(new ShapePoint(ptPos, height));
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
		points.Clear();
		points = new List<ShapePoint>();
		RemoveWalls();
		RemoveVoxels();
	}

	void RemoveWalls()
	{
		for (int i = 0; i < walls.Count; i++) {
			if (walls[i] != null) 
				DestroyImmediate(walls[i]);
		}
		walls = new List<GameObject>();
	}

	void RemoveVoxels()
	{
		for (int i = 0; i < voxels.Count; i++) {
			if (voxels[i] != null) 
				DestroyImmediate(voxels[i]);
		}
		voxels = new List<GameObject>();
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

	void BuildColliderSegment(ShapePoint pt1, ShapePoint pt2, int index, float thickness) 
	{
		// calculate the angle to rotate the collider to
		Vector3 dir = pt2.pos - pt1.pos;
		float angle = Math.AngleFromVector2(dir, -90);

		GameObject newCol = new GameObject("col " + index);
		newCol.transform.parent = transform;
		walls.Add(newCol);

		var newBox = newCol.AddComponent<BoxCollider>();

		float segmentLength = Vector3.Distance(pt1.pos, pt2.pos);
		float overlapLength = segmentLength * overlap;

		// thickness should be influenced by 'height' (which is basically what spriteShape calls border thickness)
		thickness += thicknessFromShape * pt1.height;

		newBox.size = new Vector3(segmentLength + overlapLength, height, thickness);
		newBox.center = new Vector3(segmentLength / 2, height / 2, thickness * offset);
		newBox.transform.localPosition = pt1.pos;
		newBox.transform.localEulerAngles = new Vector3(angle, -90, 90);
	}
}