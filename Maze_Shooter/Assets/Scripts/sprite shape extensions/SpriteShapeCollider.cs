using UnityEngine;
using Sirenix.OdinInspector;
using Arachnid;
using System.Collections.Generic;


[RequireComponent(typeof(SpriteShapeAnalyzer))]
public class SpriteShapeCollider : MonoBehaviour
{
	
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

	[Tooltip("Thickness of the border colliders.")]
	public float thickness = 1;

	[Range(-.5f, .5f), Tooltip("Offset of the collider from the path line. 0 means the collider will be centered over the path line.")]
	public float offset = .5f;

	[Range(-.05f, 1), Tooltip("How much edge colliders overlap as a percentage")]
	public float overlap = .15f;

	[SerializeField, ReadOnly, PropertyOrder(200)]
	List<GameObject> walls = new List<GameObject>();

	[SerializeField, ReadOnly, PropertyOrder(200)]
	List<GameObject> voxels = new List<GameObject>();

	SpriteShapeAnalyzer analyzer {
		get {
			if (_analyzer) return _analyzer;
			_analyzer = GetComponent<SpriteShapeAnalyzer>();
			return _analyzer;
		}
	}

	SpriteShapeAnalyzer _analyzer;


	Bounds GetBounds() 
	{
		analyzer.Analyze();
		Bounds bounds = new Bounds(transform.position, Vector3.one);

		for (int i = 0; i < analyzer.points.Count; i++) {
			Vector3 pt = analyzer.points[i].pos;
			bounds.Encapsulate(transform.TransformPoint(pt));
		}

		bounds.size += new Vector3(voxelWidth * 2, 0, voxelHeight * 2);

		return bounds;
	}



	
	[ButtonGroup]
	void BuildCollider()
	{
		ClearAllColliders();
		analyzer.Analyze();
		
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

	void BuildWalls(float thickness, bool addHeightThickness = true) 
	{
		for (int i = 1; i < analyzer.points.Count; i++) 
			BuildColliderSegment(analyzer.points[i-1], analyzer.points[i], i, thickness, addHeightThickness);

		// build the very last collider from last point to the first point
		if (!analyzer.IsOpenEnded) 
			BuildColliderSegment(analyzer.points[analyzer.points.Count - 1], analyzer.points[0], analyzer.points.Count, thickness, addHeightThickness);
	}


	void BuildFill()
	{
		float prevThickness = thickness;

		RemoveWalls();

		// make thin borders so we can use them to calculate
		BuildWalls(.01f, false);

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

		// remove the temporary walls that were put in place to build voxels
		RemoveWalls();

		BuildWalls(prevThickness);
	}

	Vector3 FlatVoxelPos (Vector3 input) => new Vector3(input.x, transform.position.y + height / 2, input.z);

	[ButtonGroup]
	void ClearAllColliders()
	{
		analyzer.Reset();
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

	void BuildColliderSegment(ShapePoint pt1, ShapePoint pt2, int index, float thickness, bool addHeightThickness = true) 
	{
		// calculate the angle to rotate the collider to
		Vector3 dir = pt2.pos - pt1.pos;
		float angle = Math.AngleFromVector2(dir, -90);

		GameObject newCol = new GameObject("col " + index);
		newCol.transform.parent = transform;
		walls.Add(newCol);

		var newBox = newCol.AddComponent<BoxCollider>();

		float segmentLength = Vector3.Distance(pt1.pos, pt2.pos);
		float overlapLength = addHeightThickness ? segmentLength * overlap : 0;

		// thickness should be influenced by 'height' (which is basically what spriteShape calls border thickness)
		if (addHeightThickness)
			thickness += analyzer.heightFactor * pt1.height * 2;

		newBox.size = new Vector3(segmentLength + overlapLength, height, thickness);
		newBox.center = new Vector3(segmentLength / 2, height / 2, thickness * offset);
		newBox.transform.localPosition = pt1.pos;
		newBox.transform.localEulerAngles = new Vector3(angle, -90, 90);
	}
}