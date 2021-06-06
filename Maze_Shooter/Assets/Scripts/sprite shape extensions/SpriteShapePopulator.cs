using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Arachnid;


[TypeInfoBox("use this to populate prefab collections around a path or shape.")]
[RequireComponent(typeof(SpriteShapeAnalyzer))]
public class SpriteShapePopulator : MonoBehaviour
{

	public enum ShapeMode {
		PathCenter,
		PathHeightBorders,
	}

	public ShapeMode shapeMode;

	[Tooltip("Parent of populated objects. If left empty, this transform will be used.")]
	public Transform parent;

	[Tooltip("Uses prefabs from this collection to spawn along path")]
	public SpawnCollection toSpawn;

	[PropertyOrder(250)]
	public List<GameObject> instances = new List<GameObject>();

	[Range(0.1f, 9)]
	public float spacing = .3f;

	[MinMaxSlider(0.01f, 3, true)]
	public Vector2 spawnedScale = Vector2.one;

	[MinValue(0)]
	public float randomness = .5f;

	
	public float offsetFromBorder = .1f;

	float TotalPathLength(List<Vector3> points) 
	{
		float pathLength = 0;
		for (int i = 1; i < points.Count; i++)
		{
			var p1 = points[i-1];
			var p2 = points[i];
			pathLength += Vector3.Distance(p1, p2);
		}
		return pathLength;
	}

	[ButtonGroup]
	void Populate()
	{
		if (!toSpawn) return;
		SpriteShapeAnalyzer anal = GetComponent<SpriteShapeAnalyzer>();

		List<Vector3> points = new List<Vector3>();

		anal.Analyze();

		if (shapeMode == ShapeMode.PathCenter)
			points = anal.PathPoints();

		if (shapeMode == ShapeMode.PathHeightBorders)
			points = anal.BorderPoints();

		int iterations = 0;
		Vector3 pos = points[0];
		int next = 1;
		bool breakNext = false;
		while (iterations < 9999)
		{
			// get the direction to the next 
			Vector3 nextPos = points[next];
			Vector3 dir = nextPos - pos;

			// determine final position and instantiate
			Vector3 finalPos = pos + Random.insideUnitSphere * randomness;

			// add right/left offset from border
			finalPos += Vector3.Cross(dir.normalized, Vector3.up) * offsetFromBorder;

			// Y of final pos should just be the same as this object (flat)
			finalPos = new Vector3(finalPos.x, transform.position.y, finalPos.z);
			Instantiate(finalPos);

			// crawl point towards next pos
			pos += dir.normalized * spacing;

			// check if close enough to next pos
			if (Vector3.Distance(pos, nextPos) <= spacing) {

				if (breakNext) break;

				pos = points[next];
				next++;
				if (next >= points.Count) {
					if (anal.IsOpenEnded)
						break;

					else {
						pos = points[points.Count-1];
						next = 0;
						breakNext = true;
					}
				}
			}

			iterations++;
		}
	}

	void Instantiate(Vector3 pos)
	{
		#if UNITY_EDITOR
		var prefab = toSpawn.GetRandom();
		var instance = UnityEditor.PrefabUtility.InstantiatePrefab(prefab) as GameObject;
		float newScale = Random.Range(spawnedScale.x, spawnedScale.y);
		instance.transform.localScale *= newScale;
		instance.transform.position = pos;
		instance.transform.parent = parent ? parent : transform;
		instances.Add(instance);
		#endif
	}

	[ButtonGroup]
	void Clear()
	{
		foreach (GameObject instance in instances)
			DestroyImmediate(instance);

		instances.Clear();
		instances = new List<GameObject>();
	}
}
